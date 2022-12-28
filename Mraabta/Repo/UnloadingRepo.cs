using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using MRaabta.Models;

namespace MRaabta.Repo
{
    public class UnloadingRepo
    {
        SqlConnection con;
        public UnloadingRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task OpenAsync()
        {
            await con.OpenAsync();
        }
        public void Close()
        {
            if (con.State == System.Data.ConnectionState.Open)
                con.Close();
        }
        public async Task<DropDownModel> GetBranch(string branch)
        {
            try
            {
                var query = $@"select 
                                branchCode as [Value],
                                sname as [Text]
                                from 
                                Branches where status = 1 and branchCode = {branch}
                                order by sname;";
                var rs = await con.QueryFirstOrDefaultAsync<DropDownModel>(query);
                return rs;
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<(string Product, int Prefix, int Length, int PrefixLength)>> GetConsignmentsLength()
        {
            try
            {
                var rs = await con.QueryAsync<(string Product, int Prefix, int Length, int PrefixLength)>(@"select Product, Prefix, Length, len(Prefix) as PrefixLength from MnP_ConsignmentLengths where status = 1 and Prefix is not null;", commandTimeout: int.MaxValue);
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<LoadingInfo> LoadingDetails(long lid)
        {
            try
            {
                var query = $@"select 
                            l.id as LoadingNo,
                            r.Name as Route,
                            l.origin as OriginId,
                            org.name as Origin,
                            l.[description] as Description,
                            l.courierName as Courier,
                            l.sealNo as SealNo,
                            format(l.createdOn,'dd-MMM-yyyy') as Date,
                            tt.AttributeDesc as TransportType,
                            case when l.vehicleId <> 103 then v.name else l.VehicleRegNo end as VehicleRegNo
                            from MnP_Loading l
                            inner join Branches org on org.branchCode = l.origin
                            inner join rvdbo.MovementRoute r on r.MovementRouteId = l.routeId
                            inner join rvdbo.Lookup tt on tt.AttributeGroup = 'TRANSPORT_TYPE' and tt.Id = l.transportationType
                            inner join Vehicle v on v.id = l.vehicleId
                            where l.id = '{lid}';";

                await con.OpenAsync();
                var loading = await con.QueryFirstOrDefaultAsync<LoadingInfo>(query);

                query = $@"select 
                        lc.loadingid as LoadingNo,
                        lc.consignmentNumber as BagCN,
                        case when c.consignmentNumber is not null then c.orgin else l.origin end as OriginId,
                        case when c.consignmentNumber is not null then corg.sname else lorg.sname end as Origin,
                        lc.CNDestination as DestinationId,
                        dest.sname as Destination,
                        lc.cnPieces as Pcs,
                        cast(lc.CNWeight as decimal(12,1)) as Weight,
                        lc.ServiceType,
                        isnull(ct.name,'Normal') as CNType,
                        null as SealNo,
                        0 as IsBag,
                        6 as Status,
                        lc.Remarks
                        from MnP_LoadingConsignment lc
                        inner join MnP_Loading l on l.id = lc.loadingId
                        inner join Branches lorg on lorg.branchCode = l.origin
                        inner join Branches dest on dest.branchCode = lc.CNDestination
                        left join ConsignmentType ct on ct.id = lc.ConsignmentType
                        left join consignment c on c.consignmentNumber = lc.consignmentNumber
                        left join Branches corg on corg.branchCode = c.orgin
                        where l.id = '{lid}'
                        union
                        select 
                        lb.loadingid as LoadingNo,
                        lb.bagNumber as BagCN,
                        lb.BagOrigin as OriginId,
                        org.sname as Origin,
                        lb.BagDestination as DestinationId,
                        dest.sname as Destination,
                        null as Pcs,
                        cast(lb.BagWeight as decimal(12,1)) as Weight,
                        null as ServiceType,
                        null as CNType,
                        lb.BagSeal as SealNo,
                        1 as IsBag,
                        6 as Status,
                        lb.Remarks
                        from MnP_LoadingBag lb
                        inner join MnP_Loading l on l.id = lb.loadingId
                        inner join Branches org on org.branchCode = lb.BagOrigin
                        inner join Branches dest on dest.branchCode = lb.BagDestination
                        where loadingId = '{lid}'";

                var loadingDetails = await con.QueryAsync<LoadingInfoDetails>(query);

                con.Close();

                loading.TotalWeight = loadingDetails.Sum(x => decimal.Parse(x.Weight));
                loading.BagsCount = loadingDetails.Count(x => x.IsBag);
                loading.CNsCount = loadingDetails.Count(x => !x.IsBag);
                loading.LoadingInfoDetails = loadingDetails.ToList();

                return loading;
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
        public async Task<string> DVRTSPrimaryCheck(string cn)
        {
            try
            {
                var query = $@"select top 1 xb.Msg from(
                            SELECT top(1) 1 AS SNO, 'Consignment already exist in archive database' AS Msg from primaryconsignments
                            where isManual = 1 AND consignmentnumber = '{cn}'
                            union
                            select top(1) 2 AS SNO,
                            case 
                            when Reason = '59' then concat('This CN ',consignmentNumber,' is already Mark as RS-Return to Shipper')
                            when Reason = '123' then concat('This CN ',consignmentNumber,' is already Mark as Delivered')
                            end as Msg
                            from RunsheetConsignment where Reason in ('59','123') and consignmentnumber = '{cn}'
                            union
                            select top(1) 3 AS SNO,
                            'Consignment already Marked Delivered or Returned' AS Msg 
                            from Mnp_ConsignmentOperations where ConsignmentId = '{cn}' and (IsDelivered = 3 or IsReturned = 1)
                            ) as xb ORDER BY xb.SNO;";
                var rs = await con.QueryFirstOrDefaultAsync<string>(query);
                return rs;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<(string CN, int OriginId, string Origin, int DestinationId, string Destination, double Weight, int Pieces, bool IsApproved, string ServiceType, int ConsignmentTypeId, int Status, bool ReachedDestination)> GetCN(string cn)
        {
            try
            {
                var query = $@"with t as (
                                select top 1 consignmentnumber as CN, case when Reason in ('70', '58') then 0 else 1 end AtDest
                                from RunsheetConsignment where consignmentNumber = '{cn}' 
                                order by createdOn desc
                                )
                                select top 1
                                c.consignmentnumber as CN, 
                                orgin as OriginId,
                                b1.sname as Origin,
                                c.destination as DestinationId,
                                b2.sname as Destination,
                                weight as [Weight],
                                pieces as Pieces,
                                c.isApproved as IsApproved,
                                c.serviceTypeName as [ServiceType],
                                c.consignmentTypeId as [ConsignmentTypeId],
                                c.status as [Status],
                                isnull(tv.AtDest,0)as ReachedDestination                                
                                from consignment c
                                left join t tv on c.consignmentnumber = tv.CN                                
                                inner join Branches b1 on b1.branchCode = c.orgin
                                inner join Branches b2 on b2.branchCode = c.destination
                                where c.consignmentnumber = '{cn}';";
                var rs = await con.QueryFirstOrDefaultAsync<(string CN, int OriginId, string Origin, int DestinationId, string Destination, double Weight, int Pieces, bool IsApproved, string ServiceType, int ConsignmentTypeId, int Status, bool ReachedDestination)>(query);
                return rs;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<(bool error, bool found, dynamic data, string msg)> GetBagInfo(string bagNo)
        {
            try
            {
                await con.OpenAsync();
                var query = $@"SELECT top 1
                                b.bagNumber as Bag,
                                b.totalWeight as [Weight],
                                b.origin as OriginId,
                                b.destination as DestinationId,
                                b.sealNo as SealNo,
                                b1.sname as OriginName,
                                b2.sname as DestinationName
                                FROM Bag b
                                inner JOIN Branches b1 ON b1.branchCode = b.origin
                                inner JOIN Branches b2 ON b2.branchCode = b.destination
                                WHERE b.bagNumber = '{bagNo}'";
                var rs = await con.QueryFirstOrDefaultAsync(query);
                con.Close();
                return (error: false, found: rs != null, data: rs, msg: null);
            }
            catch (SqlException ex)
            {
                return (error: true, found: false, data: null, msg: ex.Message);
            }
            catch (Exception ex)
            {
                return (error: true, found: false, data: null, msg: ex.Message);
            }
        }
        public async Task<(bool sts, long id, string msg)> SaveUnloading(Unloading model)
        {
            SqlTransaction trans = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                await con.OpenAsync();
                trans = con.BeginTransaction();
                var query = $@"insert into mnp_unloading (origin, destination, BranchCode, Createdon, CreatedBy, LocationID) values({model.OriginId},{model.DestinationId},{model.BranchCode},getdate(),{model.CreatedBy},{model.LocationId}); select SCOPE_IDENTITY() as UnloadingId;";
                var unloadingId = await con.QueryFirstOrDefaultAsync<long>(query, transaction: trans);

                foreach (var loadingId in model.UnloadingDetail.Where(x => x.LoadingNo != 0).Select(x => x.LoadingNo).Distinct())
                {
                    sb.AppendLine($"insert into MnP_UnloadingRef (unloadingID, loadingID, createdby, createdOn) values({unloadingId},{loadingId},{model.CreatedBy},getdate());");
                }

                foreach (var item in model.UnloadingDetail.Where(x => x.IsBag))
                {
                    sb.AppendLine($"insert into mnp_UnloadingBag (UnloadingID, BagNumber, BagDestination, CreatedBy, Createdon, unloadingStateID, bagWeight, bagRemarks, bagOrigin, BagSeal) values({unloadingId},'{item.BagCN}',{item.DestinationId},{model.CreatedBy},getdate(),{item.Status}, {item.Weight},'{item.Remarks}',{item.OriginId},'{item.SealNo}');");
                    sb.AppendLine($"insert into BulkTrackingData (DocumentNumber,ActivityNumber,StateId,LocationID) values('{item.BagCN}','{unloadingId}',5,{model.LocationId});");
                }

                foreach (var item in model.UnloadingDetail.Where(x => !x.IsBag))
                {
                    sb.AppendLine($"insert into mnp_UnloadingConsignment (UnLoadingID, ConsignmentNumber, CNDestination, CreatedBy, Createdon, UnloadingStateID, cnPieces, cnWeight, cnRemarks, CnOrigin) values ({unloadingId},'{item.BagCN}',{item.DestinationId},{model.CreatedBy},getdate(),{item.Status},{item.Pcs},{item.Weight},'{item.Remarks}',{model.OriginId});");
                    if (item.Status != 6)
                        sb.AppendLine($"insert into ConsignmentsTrackingHistory (ConsignmentNumber, UnloadingNumber, StateId, CurrentLocation, StatusTime, TransactionTime,loadingNumber) values('{item.BagCN}', {unloadingId}, 5, '{model.Location}', getdate(), getdate(),'{(item.LoadingNo == 0 ? "null" : item.LoadingNo.ToString())}');");
                    if (!(item.BagCN[0] == '5' && item.BagCN.Length == 15))
                    {
                        sb.AppendLine($@"if not exists(select consignmentNumber from Consignment where consignmentNumber = '{item.BagCN}')
                                        begin
                                        insert into Consignment (consignmentNumber, serviceTypeName, riderCode, consignmentTypeId, weight, orgin, destination, createdon, createdby, customerType, pieces, creditClientId, weightUnit, discount, 
                                        cod, totalAmount, zoneCode, branchCode, gst, consignerAccountNo, bookingDate, isApproved, deliveryStatus, dayType, isPriceComputed, isNormalTariffApplied, receivedFromRider)
                                        values('{item.BagCN}','overnight','','12',{item.Weight}, {item.OriginId}, {model.DestinationId}, GETDATE(), {model.CreatedBy}, '1', {item.Pcs}, '330140', '1', '0', '0', '0', '{model.ZoneCode}', {model.BranchCode}, '0', '4D1', GETDATE(), '0', '4','0','0','0','1')
                                        end;");
                    }
                }

                await con.ExecuteAsync(sb.ToString(), transaction: trans);

                trans.Commit();
                con.Close();
                return (true, unloadingId, $"Unloading# {unloadingId} saved");
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                throw ex;
            }
        }
        public async Task<UnloadingPrint> UnloadingDetails(long id)
        {
            try
            {
                await con.OpenAsync();
                var query = $@"select Id, org.name as Origin, dest.name as Destination, u.Createdon as Date from mnp_unloading u
                                inner join Branches org on org.branchCode = u.origin
                                inner join Branches dest on dest.branchCode = u.destination
                                where ID = {id}";
                var unloadingInfo = await con.QueryFirstOrDefaultAsync<UnloadingPrint>(query);

                query = $@"select 
                        l.id as LoadingNo,
                        r.Name as Route,
                        l.origin as OriginId,
                        org.name as Origin,
                        l.destination as DestinationId,
                        dest.name as Destination,
                        l.[description] as Description,
                        l.courierName as Courier,
                        l.sealNo as SealNo,
                        format(l.createdOn,'dd-MMM-yyyy') as Date,
                        tt.AttributeDesc as TransportType,
                        case when l.vehicleId <> 103 then v.name else l.VehicleRegNo end as VehicleRegNo,
                        l.FlightNo
                        from MnP_Loading l
                        inner join MnP_UnloadingRef lr on l.id = lr.loadingID
                        inner join Branches org on org.branchCode = l.origin
                        inner join Branches dest on dest.branchCode = l.destination
                        inner join rvdbo.MovementRoute r on r.MovementRouteId = l.routeId
                        inner join rvdbo.Lookup tt on tt.AttributeGroup = 'TRANSPORT_TYPE' and tt.Id = l.transportationType
                        inner join Vehicle v on v.id = l.vehicleId
                        where lr.unloadingID = {id}";

                var loadingsInfo = await con.QueryAsync<LoadingInfo>(query);

                unloadingInfo.LoadingsInfo = loadingsInfo.ToList();

                query = $@"select 
                            lc.consignmentNumber as BagCN,
                            org.sname as Origin,
                            dest.sname as Destination,
                            lc.cnPieces as Pcs,
                            cast(lc.CNWeight as decimal(12,1)) as Weight,
                            0 as IsBag,
                            lc.UnloadingStateID as Status,
                            lc.cnRemarks as Remarks
                            from mnp_UnloadingConsignment lc
                            inner join Branches org on org.branchCode = lc.CnOrigin
                            inner join Branches dest on dest.branchCode = lc.CNDestination
                            where lc.UnLoadingID = {id}
                            union
                            select 
                            lb.bagNumber as BagCN,
                            org.sname as Origin,
                            dest.sname as Destination,
                            null as Pcs,
                            cast(lb.BagWeight as decimal(12,1)) as Weight,
                            1 as IsBag,
                            lb.unloadingStateID as Status,
                            lb.bagRemarks as Reamrks
                            from mnp_UnloadingBag lb
                            inner join Branches org on org.branchCode = lb.BagOrigin
                            inner join Branches dest on dest.branchCode = lb.BagDestination
                            where lb.unloadingID = {id}";

                var loadingInfoDetails = await con.QueryAsync<LoadingInfoDetails>(query);

                unloadingInfo.LoadingInfoDetails = loadingInfoDetails.ToList();

                con.Close();

                return unloadingInfo;
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
        public async Task<List<UnloadingPrint>> GetUnloadings(string branch, string date)
        {
            try
            {
                await con.OpenAsync();
                var query = $@"select Id, org.name as Origin, dest.name as Destination, u.Createdon as Date from mnp_unloading u
                            inner join Branches org on org.branchCode = u.origin
                            inner join Branches dest on dest.branchCode = u.destination
                            where cast(u.Createdon as date) = '{date}' and u.BranchCode = {branch};";
                var rs = await con.QueryAsync<UnloadingPrint>(query);

                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
        public async Task<(List<LoadingInfo> loadings, List<LoadingInfoDetails> details)> GetUnloadingDataForEdit(long id, SqlTransaction trans = null)
        {
            try
            {
                IEnumerable<LoadingInfo> loadings = new List<LoadingInfo>();
                IEnumerable<LoadingInfoDetails> loadingDetails = new List<LoadingInfoDetails>();
                var query = $@"select 
                                l.id as LoadingNo,
                                r.Name as Route,
                                l.origin as OriginId,
                                org.name as Origin,
                                l.[description] as Description,
                                l.courierName as Courier,
                                l.sealNo as SealNo,
                                format(l.createdOn,'dd-MMM-yyyy') as Date,
                                tt.AttributeDesc as TransportType,
                                case when l.vehicleId <> 103 then v.name else l.VehicleRegNo end as VehicleRegNo,
                                1 as IsOld
                                from MnP_Loading l
                                inner join MnP_UnloadingRef ur on ur.loadingID = l.id
                                inner join Branches org on org.branchCode = l.origin
                                inner join rvdbo.MovementRoute r on r.MovementRouteId = l.routeId
                                inner join rvdbo.Lookup tt on tt.AttributeGroup = 'TRANSPORT_TYPE' and tt.Id = l.transportationType
                                inner join Vehicle v on v.id = l.vehicleId
                                where ur.unloadingID = {id};";
                var DetailQuery = $@"with t as (
                                select loadingID as LoadingId from MnP_UnloadingRef where unloadingID = {id}
                            )
                            select 
                            isnull(lc.loadingId,0) as LoadingNo,
                            uc.consignmentNumber as BagCN,
                            uc.CnOrigin as OriginId,
                            org.sname as Origin,
                            uc.CNDestination as DestinationId,
                            dest.sname as Destination,
                            uc.cnPieces as Pcs,
                            cast(uc.CNWeight as decimal(12,1)) as Weight,
                            null as SealNo,
                            0 as IsBag,
                            uc.UnloadingStateID as Status,
                            isnull(lc.Remarks,'') as Remarks,
                            1 as IsOld
                            from mnp_UnloadingConsignment uc
                            inner join Branches org on org.branchCode = uc.CnOrigin
                            inner join Branches dest on dest.branchCode = uc.CNDestination
                            left join MnP_LoadingConsignment lc on uc.ConsignmentNumber = lc.consignmentNumber and lc.loadingId in(select loadingId from t)
                            where uc.unloadingID = {id}
                            union
                            select 
                            isnull(lb.loadingId,0) as LoadingNo,
                            ub.bagNumber as BagCN,
                            ub.BagOrigin as OriginId,
                            org.sname as Origin,
                            ub.BagDestination as DestinationId,
                            dest.sname as Destination,
                            null as Pcs,
                            cast(ub.BagWeight as decimal(12,1)) as Weight,
                            ub.BagSeal as SealNo,
                            1 as IsBag,
                            ub.unloadingStateID as Status,
                            isnull(ub.bagRemarks,'') as Remarks,
                            1 as IsOld
                            from mnp_UnloadingBag ub
                            inner join Branches org on org.branchCode = ub.BagOrigin
                            inner join Branches dest on dest.branchCode = ub.BagDestination
                            left join MnP_LoadingBag lb on lb.bagNumber = ub.BagNumber and lb.loadingId in(select loadingId from t)
                            where ub.unloadingID = {id};";

                if (trans != null)
                {
                    loadings = await con.QueryAsync<LoadingInfo>(query, transaction: trans);
                    loadingDetails = await con.QueryAsync<LoadingInfoDetails>(DetailQuery, transaction: trans);
                }
                else
                {
                    loadings = await con.QueryAsync<LoadingInfo>(query);
                    loadingDetails = await con.QueryAsync<LoadingInfoDetails>(DetailQuery);
                }



                foreach (var loading in loadings)
                {
                    loading.BagsCount = loadingDetails.Count(x => x.LoadingNo == loading.LoadingNo && x.IsBag);
                    loading.CNsCount = loadingDetails.Count(x => x.LoadingNo == loading.LoadingNo && !x.IsBag);
                    loading.TotalWeight = loadingDetails.Where(x => x.LoadingNo == loading.LoadingNo).Sum(x => decimal.Parse(x.Weight));
                }

                return (loadings.ToList(), loadingDetails.ToList());
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<(bool sts, long id, string msg)> UpdateUnloading(Unloading model)
        {
            SqlTransaction trans = null;
            try
            {
                model.UnloadingDetail.ForEach(x =>
                {
                    if (x.IsBag)
                        x.SealNo = x.SealNo == null ? "" : x.SealNo;
                    x.Remarks = x.Remarks == null ? "" : x.Remarks;
                });

                StringBuilder sb = new StringBuilder();
                await con.OpenAsync();
                trans = con.BeginTransaction();
                var oldUnloading = await GetUnloadingDataForEdit(model.Id, trans);
                var currentLoadingIds = model.UnloadingDetail.Where(x => x.LoadingNo != 0).Select(x => x.LoadingNo).Distinct().ToList();

                var loadingsToAdd = currentLoadingIds.Where(x => !oldUnloading.loadings.Select(y => y.LoadingNo).Contains(x)).ToList();

                foreach (var loadingId in loadingsToAdd)
                {
                    sb.AppendLine($"insert into MnP_UnloadingRef (unloadingID, loadingID, createdby, createdOn) values({model.Id},{loadingId},{model.CreatedBy},getdate());");
                }

                var detailsToAdd = model.UnloadingDetail.Where(x => !oldUnloading.details.Select(y => y.BagCN).Contains(x.BagCN)).ToList();

                foreach (var item in detailsToAdd)
                {
                    if (item.IsBag)
                    {
                        sb.AppendLine($"insert into mnp_UnloadingBag (UnloadingID, BagNumber, BagDestination, CreatedBy, Createdon, unloadingStateID, bagWeight, bagRemarks, bagOrigin, BagSeal) values({model.Id},'{item.BagCN}',{item.DestinationId},{model.CreatedBy},getdate(),{item.Status}, {item.Weight},'{item.Remarks}',{item.OriginId},'{item.SealNo}');");
                        sb.AppendLine($"insert into BulkTrackingData (DocumentNumber,ActivityNumber,StateId,LocationID) values('{item.BagCN}','{model.Id}',5,{model.LocationId});");
                    }
                    else
                    {
                        sb.AppendLine($"insert into mnp_UnloadingConsignment (UnLoadingID, ConsignmentNumber, CNDestination, CreatedBy, Createdon, UnloadingStateID, cnPieces, cnWeight, cnRemarks, CnOrigin) values ({model.Id},'{item.BagCN}',{item.DestinationId},{model.CreatedBy},getdate(),{item.Status},{item.Pcs},{item.Weight},'{item.Remarks}',{model.OriginId});");
                        if (item.Status != 6)
                            sb.AppendLine($"insert into ConsignmentsTrackingHistory (ConsignmentNumber, UnloadingNumber, StateId, CurrentLocation, StatusTime, TransactionTime,loadingNumber) values('{item.BagCN}', {model.Id}, 5, '{model.Location}', getdate(), getdate(),'{(item.LoadingNo == 0 ? "null" : item.LoadingNo.ToString())}');");
                        if (!(item.BagCN[0] == '5' && item.BagCN.Length == 15))
                        {
                            sb.AppendLine($@"if not exists(select consignmentNumber from Consignment where consignmentNumber = '{item.BagCN}')
                                        begin
                                        insert into Consignment (consignmentNumber, serviceTypeName, riderCode, consignmentTypeId, weight, orgin, destination, createdon, createdby, customerType, pieces, creditClientId, weightUnit, discount, 
                                        cod, totalAmount, zoneCode, branchCode, gst, consignerAccountNo, bookingDate, isApproved, deliveryStatus, dayType, isPriceComputed, isNormalTariffApplied, receivedFromRider)
                                        values('{item.BagCN}','overnight','','12',{item.Weight}, {item.OriginId}, {model.DestinationId}, GETDATE(), {model.CreatedBy}, '1', {item.Pcs}, '330140', '1', '0', '0', '0', '{model.ZoneCode}', {model.BranchCode}, '0', '4D1', GETDATE(), '0', '4','0','0','0','1')
                                        end;");
                        }
                    }
                }

                var rs = await con.ExecuteAsync(sb.ToString(), transaction: trans);
                trans.Commit();
                con.Close();
                return (rs > 0, model.Id, $"Unloading# {model.Id} updated");
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    trans.Rollback();
                    con.Close();
                }
                throw ex;
            }
        }
    }
}