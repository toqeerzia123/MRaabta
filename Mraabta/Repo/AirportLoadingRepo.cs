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
    public class AirportLoadingRepo
    {
        SqlConnection con;
        public AirportLoadingRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public async Task OpenAsync()
        {
            await con.OpenAsync();
        }
        public void Close()
        {
            con.Close();
            con.Dispose();
        }
        public async Task<DropDownModel> GetBranchDetails(string branchId)
        {
            try
            {
                var query = $@"select top 1 branchCode as [Value], [name] as [Text] from Branches where branchCode = {branchId}";
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
        public async Task<List<DropDownModel>> GetRoutes(string branchId)
        {
            try
            {
                var query = $@"select MovementRouteId as [Value],
                            [Name] as [Text]
                            from rvdbo.MovementRoute MR where
                            MR.MovementRouteId='180'  --Only KHI to LAO Route
                            and MR.ParentMovementRouteId is null and MR.IsActive = '1';";
                var rs = await con.QueryAsync<DropDownModel>(query);
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
        public async Task<List<DropDownModel>> GetRoutesTouchpoints(string branchId, string routeId)
        {
            try
            {
                var query = $@"select MovementRouteId as [Value], [Name] as [Text] from rvdbo.MovementRoute mr where 
                                mr.OriginBranchId = {branchId}
                                and mr.ParentMovementRouteId = '{routeId}' 
                                OR mr.MovementRouteId = '{routeId}' 
                                and MR.IsActive = '1' order by name;";
                var rs = await con.QueryAsync<DropDownModel>(query);
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
        public async Task<List<DropDownModel>> GetRouteDestinations(string routeId)
        {
            try
            {
                var query = $@"select MR.DestBranchId as [Value], b.name as [Text]
                            from rvdbo.MovementRoute MR
                            inner join Branches b on b.branchCode = MR.DestBranchId
                            where MR.MovementRouteId = '{routeId}' and b.status = '1' 
                            ORDER BY b.name;";
                var rs = await con.QueryAsync<DropDownModel>(query);
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
        public async Task<List<DropDownModel>> GetTransportType()
        {
            try
            {
                var query = $@"select l.id as [Value], l.AttributeDesc as [Text] from rvdbo.Lookup l where l.AttributeGroup = 'TRANSPORT_TYPE'
                              and l.Id='25' -- Only By Air- MPL;";
                var rs = await con.QueryAsync<DropDownModel>(query);
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
        public async Task<List<DropDownModel>> GetVehicleType()
        {
            try
            {
                var query = $@"select TypeID as [Value], TypeDesc as [Text] from Vehicle_Type where status = '1'";
                var rs = await con.QueryAsync<DropDownModel>(query);
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
        public async Task<List<DropDownModel>> GetVehicles()
        {
            try
            {
                var query = $@"select v.VehicleCode as [Value], v.MakeModel as [Text] from rvdbo.Vehicle v where v.IsActive = 1 order by VehicleId;";
                var rs = await con.QueryAsync<DropDownModel>(query);
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
        public async Task<List<DropDownModel>> GetBranches()
        {
            try
            {
                var query = $@"select branchCode as [Value], concat(sname,' - ',name) as [Text] from Branches where status = 1 order by sname;";
                var rs = await con.QueryAsync<DropDownModel>(query);
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
                            where l.id = '{lid}' ;";

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
                        lc.UnloadingStateID as Status,
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
                        lb.UloadingStateID as Status,
                        lb.Remarks
                        from MnP_LoadingBag lb
                        inner join MnP_Loading l on l.id = lb.loadingId
                        inner join Branches org on org.branchCode = lb.BagOrigin
                        inner join Branches dest on dest.branchCode = lb.BagDestination
                        where loadingId = '{lid}' ";

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
        public async Task<LoadingInfo> UnLoadingDetails(long lid)
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
								from mnp_unloading l
								inner join Branches org on org.branchCode = l.origin
								left join rvdbo.MovementRoute r on r.MovementRouteId = l.routeId
								left join rvdbo.Lookup tt on tt.AttributeGroup = 'TRANSPORT_TYPE' and tt.Id = l.transportationType
								left join Vehicle v on v.id = l.vehicleId
                            where l.id = '{lid}' ;";

                await con.OpenAsync();
                var loading = await con.QueryFirstOrDefaultAsync<LoadingInfo>(query);

                query = $@"select lc.UnLoadingID as LoadingNo,
                            lc.consignmentNumber as BagCN,
                            case when c.consignmentNumber is not null then c.orgin else l.origin end as OriginId,
                            case when c.consignmentNumber is not null then corg.sname else lorg.sname end as Origin,
                            lc.CNDestination as DestinationId,
                            dest.sname as Destination,
                            lc.cnPieces as Pcs,
                            cast(lc.CNWeight as decimal(12,1)) as Weight,
                            null as SealNo,
                            0 as IsBag,
                            lc.UnloadingStateID as Status,
                            lc.cnRemarks as Remarks
                            from mnp_UnloadingConsignment lc
                            inner join mnp_unloading l on l.id = lc.UnLoadingID
                            inner join Branches lorg on lorg.branchCode = l.origin
                            inner join Branches dest on cast(dest.branchCode as varchar) = lc.CNDestination
                            inner join consignment c on c.consignmentNumber = cast(lc.consignmentNumber as varchar)
                            inner join Branches corg on corg.branchCode = c.orgin
                            where l.id = '{lid}' and lc.UnloadingStateID != '6' 
                            union
                        select
	                        lb.UnloadingID as LoadingNo,
	                        lb.bagNumber as BagCN,
	                        lb.BagOrigin as OriginId,
	                        org.sname as Origin,
	                        lb.BagDestination as DestinationId,
	                        dest.sname as Destination,
	                        null as Pcs,
	                        cast(lb.BagWeight as decimal(12,1)) as Weight,                        
	                        lb.BagSeal as SealNo,
	                        1 as IsBag,
	                        lb.unloadingStateID as Status,
	                        lb.bagRemarks as Remarks
	                        from mnp_UnloadingBag lb
	                        inner join mnp_unloading l on l.id = lb.UnloadingID
	                        inner join Branches org on org.branchCode = lb.BagOrigin
	                        inner join Branches dest on dest.branchCode = lb.BagDestination
                            where UnloadingID = '{lid}' and lb.UnloadingStateID != '6'  ";

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
                                b1.sname + '-' + b1.name as OriginName,
                                b2.sname + '-' + b2.name as DestinationName
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
        public async Task<string> InsertLoading(AirLoadingModel model)
        {
            SqlTransaction trans = null;
            try
            {
                var dtnow = DateTime.Now;
                var id = dtnow.Year.ToString().Substring(2, 2) + dtnow.Month.ToString("d2") + dtnow.Day.ToString("d2") + dtnow.Hour.ToString("D2") + dtnow.Minute.ToString("D2") + dtnow.Second.ToString("D2") + dtnow.Millisecond.ToString("D3");
                var query = $@"INSERT INTO MnP_Loading 
                                (
                                id, 
                                date, 
                                description, 
                                transportationType, 
                                vehicleId, 
                                courierName, 
                                origin, 
                                destination, 
                                expressCenterCode, 
                                branchCode, 
                                zoneCode, 
                                createdBy, 
                                createdOn, 
                                routeId, 
                                VehicleRegNo, 
                                sealNo, 
                                FlightNo, 
                                DepartureFlightDate, 
                                VehicleType, 
                                IsMaster,
                                LocationiD,
                                IsAirport,
                                TotalWeight
                                ) 
                                values(
                                '{id}', 
                                '{dtnow.ToString("yyyy-MM-dd")}', 
                                '{model.Description}', 
                                {model.TransportType}, 
                                {(!model.IsRented ? model.VehicleId.ToString() : "103")}, 
                                '{model.CourierName}', 
                                {model.BranchCode}, 
                                '{model.Destination}', 
                                '{model.ECCode}', 
                                {model.BranchCode}, 
                                {model.ZoneCode}, 
                                {model.CreatedBy}, 
                                getdate(),
                                {model.RouteId}, 
                                {(model.IsRented ? $"'{model.VehicleRegNo}'" : "null")}, 
                                '{model.LoadingSealNo}', 
                                null, 
                                null, 
                                {model.VehicleType}, 
                                1,
                                {model.LocationId},
                                0,
                                {model.UnloadingDetail.Sum(x => x.Weight)}
                                );";
                await con.OpenAsync();
                trans = con.BeginTransaction();
                var rs = await con.ExecuteAsync(query, transaction: trans);

                if (rs > 0)
                {
                    StringBuilder queryBuilder = new StringBuilder();
                    foreach (var item in model.UnloadingDetail.Where(x => x.IsBag))
                    {
                        var q = $@"INSERT INTO MnP_LoadingBag 
                                    (
                                    loadingId, 
                                    bagNumber, 
                                    BagDestination, 
                                    createdBy, 
                                    createdOn, 
                                    Remarks, 
                                    BagWeight, 
                                    BagOrigin, 
                                    BagSeal, 
                                    sortOrder
                                    )
                                    values(
                                    '{id}', 
                                    '{item.BagCN}', 
                                    {item.DestinationId}, 
                                    {model.CreatedBy}, 
                                    getDate(), 
                                    '{item.Remarks}', 
                                    {item.Weight}, 
                                    {item.OriginId}, 
                                    '{item.SealNo}', 
                                    {(item.SortOrder != null ? $"'{item.SortOrder}'" : "null")}
                                    );";

                        queryBuilder.AppendLine(q);
                        queryBuilder.AppendLine($"insert into BulkTrackingData (DocumentNumber,ActivityNumber,StateId,LocationID) values('{item.BagCN}','{id}',4,{model.LocationId});");
                    }

                    foreach (var item in model.UnloadingDetail.Where(x => !x.IsBag))
                    {
                        var q = $@"INSERT INTO MnP_LoadingConsignment 
                                    (
                                    loadingId, 
                                    consignmentNumber, 
                                    CNDestination, 
                                    createdBy, 
                                    createdOn, 
                                    Remarks, 
                                    cnPieces, 
                                    CNWeight, 
                                    SortOrder,
                                    ismerged
                                    ) 
                                    VALUES(
                                        '{id}',
                                        '{item.BagCN}',
                                        {item.DestinationId}, 
                                        {model.CreatedBy},
                                        getDate(),
                                        '{item.Remarks}',
                                        {item.Pcs},
                                        {item.Weight},
                                        {(item.SortOrder != null ? $"'{item.SortOrder}'" : "null")},
                                        0)";

                        queryBuilder.AppendLine(q);
                        queryBuilder.AppendLine($"insert into ConsignmentsTrackingHistory (ConsignmentNumber, loadingNumber, StateId, CurrentLocation, StatusTime, TransactionTime) values('{item.BagCN}', {id}, 4, '{model.Location}', getdate(), getdate());");
                    }

                    query = queryBuilder.ToString();

                    await con.ExecuteAsync(query, transaction: trans);
                }

                trans.Commit();
                con.Close();
                return id;
            }
            catch (SqlException ex)
            {
                trans.Rollback();
                con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                con.Close();
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
                                b1.name as Origin,
                                c.destination as DestinationId,
                                b2.name as Destination,
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
        public async Task<(LoadingModel loading, List<LoadingDetailModel> details)> GetLoadingDataForEdit(long id, SqlTransaction trans = null)
        {
            try
            {
                var query = $@"select 
                                l.id as LoadingNo,
                                l.routeId as RouteId,
                                l.origin as OriginId,
                                l.destination as Destination,
                                l.[description] as Description,
                                l.courierName as CourierName,
                                l.sealNo as LoadingSealNo,
                                l.transportationType as TransportType,
                                l.vehicleId as VehicleId,
                                isnull(l.VehicleRegNo,'') as VehicleRegNo,
                                isnull(l.TotalWeight,0) as TotalWeight,
                                case when l.vehicleId = 103 then 1 else 0 end as IsRented,
                                l.VehicleType as VehicleType
                                from MnP_Loading l
                                where l.id = {id}";

                var loading = await con.QueryFirstOrDefaultAsync<LoadingModel>(query, transaction: trans);

                query = $@"select 
                            lc.consignmentNumber as BagCN,
                            l.origin as OriginId,
                            org.sname as OriginName,
                            lc.CNDestination as DestinationId,
                            dest.sname as Destination,
                            cast(lc.cnPieces as nvarchar) as SealNoPieces,
                            cast(lc.CNWeight as decimal(12,1)) as Weight,
                            lc.Remarks,
                            0 as IsBag,
                            lc.SortOrder,
                            1 as IsOld
                            from MnP_LoadingConsignment lc
                            inner join MnP_Loading l on l.id = lc.loadingId
                            inner join Branches org on org.branchCode = l.origin
                            inner join Branches dest on dest.branchCode = lc.CNDestination
                            where l.id = {id}
                            union
                            select 
                            lb.bagNumber as BagCN,
                            lb.BagOrigin as OriginId,
                            org.sname as OriginName,
                            lb.BagDestination as DestinationId,
                            dest.sname as Destination,
                            lb.BagSeal as SealNoPieces,
                            cast(lb.BagWeight as decimal(12,1)) as Weight,
                            lb.Remarks,
                            1 as IsBag,
                            lb.SortOrder,
                            1 as IsOld
                            from MnP_LoadingBag lb
                            inner join MnP_Loading l on l.id = lb.loadingId
                            inner join Branches org on org.branchCode = lb.BagOrigin
                            inner join Branches dest on dest.branchCode = lb.BagDestination
                            where loadingId = {id}";

                var details = await con.QueryAsync<LoadingDetailModel>(query, transaction: trans);

                return (loading, details.ToList());
            }
            catch (SqlException ex)
            {
                return (null, null);
            }
            catch (Exception ex)
            {
                return (null, null);
            }
        }
        public async Task<List<dynamic>> GetLoadings(string branch, DateTime date, string loadingNo, string sealNo, string destinationId, string transportType)
        {
            try
            {
                var query = $@"select l.id as Id, 
                                lu.AttributeDesc as TransportType, 
                                CONVERT(VARCHAR(10), l.date, 105) as [Date], 
                                v.MakeModel + ' (' + v.Description + ')' as [VehicleName], 
                                l.courierName as [CourierName], 
                                b1.name as [Origin], 
                                b2.name as [Destination], 
                                l.description as [Description], 
                                l.flightNo as [FlightNo], 
                                l.sealno as [SealNo], 
                                CONVERT(VARCHAR(10), l.departureflightdate, 105) as [DepartureFlightDate],
                                ((SELECT ISNULL(SUM(CAST(lb.BagWeight AS FLOAT)),0) FROM MnP_LoadingBag lb WHERE lb.loadingId  = l.id) +  
                                (SELECT ISNULL(SUM(CAST(lb.CNWeight AS FLOAT)),0) FROM mnp_loadingConsignment lb WHERE lb.loadingId = l.id)) as TotalWeight, 
                                Case when isairport ='1' then 'Yes' else 'No' end as AtAirport,  
                                u.Name as CreatedBy,
                                (select top 1 COUNT(*) from MnP_UnloadingRef where loadingID = l.id) as IsUnloaded
                                from mnp_Loading l
                                left outer join rvdbo.Lookup lu 
                                on lu.id = l.transportationType 
                                and lu.AttributeGroup = 'TRANSPORT_TYPE' 
                                left outer join rvdbo.Vehicle v 
                                on v.VehicleCode = CAST(l.vehicleId AS VARCHAR) 
                                inner join Branches b1 
                                on b1.branchCode = l.origin 
                                inner join Branches b2 
                                on b2.branchCode = l.destination 
                                inner join ZNI_USER1 u on u.u_id = l.createdBy 
                                WHERE l.branchCode = {branch} 
                                and l.date = '{date.ToString("yyyy-MM-dd")}'
                                {(!string.IsNullOrEmpty(loadingNo) ? $"and l.id = {loadingNo}" : "")}
                                {(!string.IsNullOrEmpty(sealNo) ? $"and l.sealNo = '{sealNo}'" : "")}
                                {(!string.IsNullOrEmpty(destinationId) ? $"and l.destination = '{destinationId}'" : "")}
                                {(!string.IsNullOrEmpty(transportType) ? $"and l.transportationType = '{transportType}'" : "")};";

                await con.OpenAsync();
                var rs = await con.QueryAsync(query, commandTimeout: int.MaxValue);
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
        public async Task<LoadingPrintModel> GetLoading(long id)
        {
            try
            {
                await con.OpenAsync();
                var query = $@"select top 1
                            tt.AttributeDesc as TransportType,
                            case 
                            when l.vehicleId = '103' then concat('('+v.RegNo+') ',l.VehicleRegNo)
                            else v.RegNo end as VehicleRegNo,
                            l.courierName as CourierName,
                            l.description as [Description],
                            vt.TypeDesc as VehicleType,
                            o.name as Origin,
                            d.name as Destination,
                            mm.Name as Route,
                            l.sealNo as SealNo,
                            case when l.IsAirport = 1 then 'Yes' else 'No' end as IsAirport,
                            l.DepartureFlightDate,
                            l.FlightNo,
                            l.date as [Date]
                            from MnP_Loading l
                            inner join Branches o on o.branchCode = l.origin
                            inner join Branches d on d.branchCode = l.destination
                            inner join rvdbo.Movementroute mm on l.routeid = mm.movementrouteid
                            left JOIN Vehicle_Type vt ON vt.TypeID = l.VehicleType 
                            inner join rvdbo.Vehicle v on v.VehicleCode = l.vehicleId
                            inner join rvdbo.Lookup tt on tt.id = l.transportationType and tt.AttributeGroup = 'TRANSPORT_TYPE'
                            where l.id = {id};";
                var rs = await con.QueryFirstOrDefaultAsync<LoadingPrintModel>(query);

                query = $@"select 
                            b.bagNumber as BagNo,
                            cast(b.BagWeight as float) as BagWeight,
                            o.name as Origin,
                            d.name as Destination,
                            b.BagSeal,
                            isnull(b.Remarks,'') as Remarks,
                            u.Name as CreatedBy,
                            pd.Name as Product,
                            sv.Name as Service,
                            bg.[Type],
                            bg.SHS
                            from MnP_LoadingBag b
                            inner join Bag bg on b.bagNumber = bg.bagNumber
                            inner join Branches o on o.branchCode = b.BagOrigin
                            inner join Branches d on d.branchCode = b.BagDestination
                            inner join ZNI_USER1 u on u.U_ID = b.createdBy
                            inner join Services sv on sv.Id = bg.Service
                            inner join Products pd on pd.Id = sv.ProductId
                            where b.loadingId = {id};";
                var rs2 = await con.QueryAsync<LoadingPrintBagModel>(query);

                query = $@"select 
                            lc.consignmentNumber as CN,
                            lc.cnPieces as Pcs,
                            cast(lc.CNWeight as float) as Weight,
                            case when c.consignmentNumber is not null then  co.name else lo.name end as Origin,
                            d.name as Destination,
                            c.consigner as Consigner,
                            isnull(lc.Remarks,'') as Remarks,
                            u.Name as CreatedBy
                            from MnP_LoadingConsignment lc
                            inner join MnP_Loading l on l.id = lc.loadingId
                            inner join Branches lo on lo.branchCode = l.origin
                            inner join Branches d on d.branchCode = lc.CNDestination
                            inner join ZNI_USER1 u on u.U_ID = lc.createdBy
                            left join Consignment c on c.consignmentNumber = lc.consignmentNumber
                            left join Branches co on co.branchCode = c.orgin
                            where lc.loadingId = {id};";
                var rs3 = await con.QueryAsync<LoadingPrintCNModel>(query);
                con.Close();

                rs.LoadingPrintBags = rs2.ToList();
                rs.LoadingPrintCNs = rs3.ToList();

                return rs;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
        public async Task<long> UpdateLoading(AirLoadingModel model)
        {
            SqlTransaction trans = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                await con.OpenAsync();
                trans = con.BeginTransaction();

                var oldLoading = await GetLoadingDataForEdit(model.Id, trans);

                var detailsToAdd = model.UnloadingDetail.Where(x => !oldLoading.details.Select(y => y.BagCN).Contains(x.BagCN)).ToList();

                foreach (var item in detailsToAdd)
                {
                    if (item.IsBag)
                    {
                        sb.AppendLine($@"INSERT INTO MnP_LoadingBag 
                                    (
                                    loadingId, 
                                    bagNumber, 
                                    BagDestination, 
                                    createdBy, 
                                    createdOn, 
                                    Remarks, 
                                    BagWeight, 
                                    BagOrigin, 
                                    BagSeal, 
                                    sortOrder
                                    )
                                    values(
                                    '{model.Id}', 
                                    '{item.BagCN}', 
                                    {item.DestinationId}, 
                                    {model.CreatedBy}, 
                                    getDate(), 
                                    '{item.Remarks}', 
                                    {item.Weight}, 
                                    {item.OriginId}, 
                                    '{item.SealNo}', 
                                    {item.SortOrder}
                                    );");
                        sb.AppendLine($"insert into BulkTrackingData (DocumentNumber,ActivityNumber,StateId,LocationID) values('{item.BagCN}','{model.Id}',4,{model.LocationId});");
                    }
                    else
                    {
                        sb.AppendLine($@"INSERT INTO MnP_LoadingConsignment 
                                    (
                                    loadingId, 
                                    consignmentNumber, 
                                    CNDestination, 
                                    createdBy, 
                                    createdOn, 
                                    Remarks, 
                                    cnPieces, 
                                    CNWeight, 
                                    SortOrder,
                                    ismerged
                                    ) 
                                    VALUES(
                                        '{model.Id}', 
                                        '{item.BagCN}', 
                                        {item.DestinationId}, 
                                        {model.CreatedBy}, 
                                        getDate(), 
                                        '{item.Remarks}', 
                                        {item.SealNo}, 
                                        {item.Weight}, 
                                        {item.SortOrder},
                                        0
                                    );");
                        sb.AppendLine($"insert into ConsignmentsTrackingHistory (ConsignmentNumber, loadingNumber, StateId, CurrentLocation, StatusTime, TransactionTime) values('{item.BagCN}', {model.Id}, 4, '{model.Location}', getdate(), getdate());");
                    }
                }

                await con.ExecuteAsync(sb.ToString(), transaction: trans);

                trans.Commit();
                con.Close();
                return model.Id;
            }
            catch (SqlException ex)
            {
                trans.Rollback();
                con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                con.Close();
                throw ex;
            }
        }
        public async Task<int> IsUnloaded(long id)
        {
            var rs = await con.QueryFirstOrDefaultAsync<int>($@"select top 1 COUNT(*) from MnP_UnloadingRef where loadingID = {id}");
            return rs;
        }
        public async Task<List<int>> GetRunnerRoutes(int routeId)
        {
            try
            {
                await con.OpenAsync();
                var query = $@"select 
                            Route_ChildID as RunnerRoute
                            from RunnerRoutes_Child
                            where MovementRouteCode = {routeId} and [status] = 1;";
                var rs = await con.QueryAsync<int>(query);
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
    }
}