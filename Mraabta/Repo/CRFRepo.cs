using Dapper;
using MRaabta.Models;
using MRaabta.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRaabta.Repo
{
    public class CRFRepo : GeneralRepo
    {
        SqlConnection con;
        SqlConnection codcon;
        public CRFRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            codcon = new SqlConnection(ConfigurationManager.ConnectionStrings["cod"].ConnectionString);
            SetCon(con);
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
        public async Task<List<DropDownModel>> Industries()
        {
            try
            {
                var query = $@"select Id as Value, Name as Text from tblAdminIndustry where IsActvie = 1;";
                var rs = await con.QueryAsync<DropDownModel>(query);
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                Close();
                throw ex;
            }
            catch (Exception ex)
            {
                Close();
                throw ex;
            }
        }
        public async Task<List<DropDownModel>> Banks()
        {
            try
            {
                var query = $@"select Id as Value, Name as Text from Banks;";
                var rs = await con.QueryAsync<DropDownModel>(query);
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                Close();
                throw ex;
            }
            catch (Exception ex)
            {
                Close();
                throw ex;
            }
        }
        public async Task<List<DropDownModel>> Groups()
        {
            try
            {
                var query = $@"SELECT top 1000 id as Value, name as Text from ClientGroups WHERE [status] = 1;";
                var rs = await con.QueryAsync<DropDownModel>(query);
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                Close();
                throw ex;
            }
            catch (Exception ex)
            {
                Close();
                throw ex;
            }
        }
        public async Task<List<dynamic>> CustomerProducts()
        {
            try
            {
                var query = $@"select 
                                cp.Id as ProductId,
                                cp.Name as ProductName
                                from CustomerProducts cp
                                where cp.IsActive = 1;";
                var rs = await con.QueryAsync(query);
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                Close();
                throw ex;
            }
            catch (Exception ex)
            {
                Close();
                throw ex;
            }
        }
        public async Task<int> Save(Customer model, UserModel u)
        {
            SqlTransaction trans = null;
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();

                var userTypes = await GetUserRoles(trans, model);

                var query = $@"insert into Customers (BusinessName,IndustryId,ContactPerson,Designation,PhoneNo,FaxNo,CNIC,NTNRegistered,NTNNumber,GSTRegistered,GSTNumber,CityId,PostalCode,Area,SectorOrZone,Street,PlotNo,BuildingName,FloorNo,HouseOrOfficeNo,BeneficiaryName,BeneficiaryRelation,IBAN,BankId,BankBranchCode,BankBranchCity,ExpectedRevenue,IsFAF,IsFAC,FuelFactor,InvoicingScheduler,IsAutoRecovery,CreditTermsOrDays,SalesDecision,BillingInstruction,AnnualRateRevision,EInvoicing,PrintInvoice,MinimumBilling,RecoveryMode,TaxExcemption,CreatedBy,Email,Level,IsSingle,GroupId)
                                values(@BusinessName,{model.IndustryId},'{model.ContactPerson}','{model.Designation}','{model.PhoneNo}','{model.FaxNo}','{model.CNIC}',{(model.NTNRegistered ? 1 : 0)}, {(model.NTNRegistered ? $"'{model.NTNNumber}'" : "null")} ,{(model.GSTRegistered ? 1 : 0)},{(model.GSTRegistered ? $"'{model.GSTNumber}'" : "null")},{model.CityId},'{model.PostalCode}','{model.Area}','{model.SectorOrZone}','{model.Street}','{model.PlotNo}','{model.BuildingName}','{model.FloorNo}','{model.HouseOrOfficeNo}','{model.BeneficiaryName}','{model.BeneficiaryRelation}','{model.IBAN}',{model.BankId},'{model.BankBranchCode}',{model.BankBranchCity},{model.ExpectedRevenue},{(model.IsFAF ? 1 : 0)},{(model.IsFAC ? 1 : 0)},{model.FuelFactor},'{model.InvoicingScheduler}',{(model.IsAutoRecovery ? 1 : 0)},{model.CreditTermsOrDays},{model.SalesDecision},{model.BillingInstruction},{model.AnnualRateRevision},{(model.EInvoicing ? 1 : 0)},{(model.PrintInvoice ? 1 : 0)},{model.MinimumBilling},{model.RecoveryMode},{(model.TaxExcemption ? 1 : 0)},{u.Uid},'{model.Email}',{(int)userTypes.customerlevel},{model.IsSingle},{model.GroupId});
                                select SCOPE_IDENTITY() as Id;";
                var id = await con.QueryFirstOrDefaultAsync<int>(query, new { @BusinessName = model.BusinessName }, commandTimeout: int.MaxValue, transaction: trans);

                if (id > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var item in model.ContactPersons)
                    {
                        query = $@"insert into CustomerContactPersons (CustomerId,ContactPersonName,Designation,MobileNo,Email,CreatedBy)
                                values ({id},'{item.ContactPersonName}','{item.Designation}','{item.MobileNo}','{item.Email}',{u.Uid});";
                        sb.AppendLine(query);
                    }
                    await con.ExecuteAsync(sb.ToString(), commandTimeout: int.MaxValue, transaction: trans);
                    sb.Clear();

                    foreach (var item in model.PickupLocations)
                    {
                        query = $@"insert into CustomerPickupLocations (CustomerId,LocationName,ContactPersonName,MobileNo,Email,CityId,Area,SectorORZone,Street,PlotNumber,BuildingName,FloorNO,HouseOrOfficeNo,CreatedBy)
                                   values({id},'{item.LocationName}','{item.ContactPersonName}','{item.MobileNo}','{item.Email}',{item.CityId},'{item.Area}','{item.SectorORZone}','{item.Street}','{item.PlotNumber}','{item.BuildingName}','{item.FloorNO}','{item.HouseOrOfficeNo}',{u.Uid});";
                        sb.AppendLine(query);
                    }
                    await con.ExecuteAsync(sb.ToString(), commandTimeout: int.MaxValue, transaction: trans);
                    sb.Clear();

                    foreach (var item in model.Rates)
                    {
                        query = $@"insert into CustomerProductRates (CustomerId,Overnight,SecondDay,ECargo,Flyer,FlyerWind,ExpectedRev,ZeroToPoint5KGWC,ZeroToPoint5KGSZ,ZeroToPoint5KGDZ,Point5To1KGWC,Point5To1KGSZ,Point5To1KGDZ,AddKGWC,AddKGSZ,AddKGDZ,SecDayMin,SecDayAdd,ZoneAMin,ZoneAAdd,ZoneBMin,ZoneBAdd,FlyerS,FlyerM,FlyerL,FlyerXL,FlyerWinS,FlyerWinM,FlyerWinL,FlyerWinXL,ProductId,AddFactor,MinWeight)
                                    values({id},{(item.Overnight ? 1 : 0)},{(item.SecondDay ? 1 : 0)},{(item.ECargo ? 1 : 0)},{(item.Flyer ? 1 : 0)},{(item.FlyerWind ? 1 : 0)},{item.ExpectedRev},{item.ZeroToPoint5KGWC},{item.ZeroToPoint5KGSZ},{item.ZeroToPoint5KGDZ},{item.Point5To1KGWC},{item.Point5To1KGSZ},{item.Point5To1KGDZ},{item.AddKGWC},{item.AddKGSZ},{item.AddKGDZ},{item.SecDayMin},{item.SecDayAdd},{item.ZoneAMin},{item.ZoneAAdd},{item.ZoneBMin},{item.ZoneBAdd},{item.FlyerS},{item.FlyerM},{item.FlyerL},{item.FlyerXL},{item.FlyerWinS},{item.FlyerWinM},{item.FlyerWinL},{item.FlyerWinXL},{item.ProductId},{item.AddFactor},{item.MinWeight});";
                        sb.AppendLine(query);
                    }
                    await con.ExecuteAsync(sb.ToString(), commandTimeout: int.MaxValue, transaction: trans);
                    sb.Clear();
                }

                query = $@"insert into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) values
                           ({id},{u.Uid},1,1,0),
                           ({id},{ (userTypes.detaillevel == CRFUserType.ZA ? $@"(select top 1 u.U_ID from ZNI_USER1 u
                                inner join UserStaffLevels usl on usl.UserId = u.U_ID
                                inner join MNP_StaffLevel sl on sl.Id = usl.StaffLevelId
                                where u.ZoneCode = '{u.ZoneCode}' and sl.[Level] = 5)" : $@"(select distinct u.U_ID from ZNI_USER1 u
                                inner join UserStaffLevels usl on u.U_ID = usl.UserId
                                inner join MNP_StaffLevel sl on sl.Id = usl.StaffLevelId
                                where u.ZoneCode = '{u.ZoneCode}' and sl.[Level] = 2)") },{(userTypes.detaillevel == CRFUserType.ZA ? 5 : 2)},2,1);";

                await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);

                trans.Commit();
                con.Close();

                return id;
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
        public async Task<Customer> Print(int id)
        {
            try
            {
                await con.OpenAsync();
                var query = $@"select
                            c.Id,
                            c.BusinessName,
                            i.Name as Industry,
                            c.ContactPerson,
                            c.Designation,
                            c.PhoneNo,
                            c.FaxNo,
                            c.CNIC,
                            case when c.NTNRegistered = 1 then c.NTNNumber else 'Not Registered' end as NTNNumber,
                            case when c.GSTRegistered = 1 then c.GSTNumber else 'Not Registered' end as GSTNumber,
                            z.name as Zone,
                            b.name as City,
                            c.PostalCode,
                            c.Area,
                            c.SectorOrZone,
                            c.Street,
                            c.PlotNo,
                            c.BuildingName,
                            c.FloorNo,
                            c.HouseOrOfficeNo,
                            c.BeneficiaryName,
                            c.BeneficiaryRelation,
                            c.IBAN,
                            bnk.Name as BankName,
                            c.BankBranchCode,
                            pc.Name as BankBranchCityName,
                            c.ExpectedRevenue,
                            c.IsFAF,
                            c.IsFAC,
                            c.FuelFactor,
                            case when c.InvoicingScheduler = 'D' then 'Daily' 
                            when c.InvoicingScheduler = 'W' then 'Weekly' 
                            when c.InvoicingScheduler = 'M' then 'Monthly' end as InvoicingScheduler,
                            c.IsAutoRecovery,
                            c.CreditTermsOrDays,
                            c.SalesDecision,
                            c.BillingInstruction,
                            c.AnnualRateRevision,
                            c.EInvoicing,
                            c.PrintInvoice,
                            c.MinimumBilling,
                            c.RecoveryMode,
                            c.TaxExcemption,
                            c.Email,
                            u.Name as CreatedBy,
                            u.U_NAME as CreatedByEmail,
                            case when c.IsSingle = 1 then 1 else 0 end as IsSingle,
                            cg.name as [Group]
                            from Customers c
                            inner join tblAdminIndustry i on c.IndustryId = i.Id and i.IsActvie = 1
                            inner join Branches b on b.branchCode = c.CityId and b.[status] = 1
                            inner join Zones z on z.zoneCode = b.zoneCode and z.[status] = 1
                            inner join ZNI_USER1 u on u.U_ID = c.CreatedBy
                            left join Banks bnk on c.BankId = bnk.Id 
                            left join PakCities pc on pc.Id = c.BankBranchCity
                            left join ClientGroups cg on cg.id = c.GroupId
                            where c.Id = {id};
                            select 
                            cr.*, cp.Name as ProductName
                            from CustomerProductRates cr
                            inner join CustomerProducts cp on cr.ProductId = cp.Id
                            where CustomerId = {id};";
                using (var rs = await con.QueryMultipleAsync(query, commandTimeout: int.MaxValue))
                {
                    var data = await rs.ReadFirstOrDefaultAsync<Customer>();
                    if (data != null)
                        data.Rates = (await rs.ReadAsync<Rate>()).ToList();
                    con.Close();
                    return data;
                }
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
        public async Task<(List<CustomerListModel> data, CRFUserType type, List<Rate> levelRates)> Customers(UserModel u)
        {
            try
            {
                List<CustomerListModel> rs = new List<CustomerListModel>();
                await con.OpenAsync();
                var query = $@"select distinct Level
                                from UserStafflevels usl
                                inner join MNP_StaffLevel sl on sl.Id = usl.StaffLevelId
                                where usl.UserId = {u.Uid} and usl.IsActive = 1;";
                var userType = await con.QueryFirstOrDefaultAsync<CRFUserType>(query);

                if (userType == CRFUserType.SalesPerson)
                {
                    rs = await SPCustomers(u.Uid);
                }
                if (userType == CRFUserType.AreaManager)
                {
                    rs = await AMCustomers(u.Uid);
                }
                if (userType == CRFUserType.GeneralManager)
                {
                    rs = await GMCustomers(u);
                }
                if (userType == CRFUserType.Director)
                {
                    query = $@"select distinct sl.ProductId from UserStaffLevels usl
                            inner join MNP_StaffLevel sl on usl.StaffLevelId = sl.Id
                            where UserId = {u.Uid};";
                    var products = (await con.QueryAsync<int>(query)).ToList();
                    rs = await DirectorCustomers(u.Uid, u, products);
                }
                else if (userType == CRFUserType.ZA)
                {
                    rs = await ZACustomers(u.Uid);
                }

                query = "select * from ProductRates where [Level] = 1;";
                var lvlrates = (await con.QueryAsync<Rate>(query)).ToList();
                con.Close();

                return (rs, userType, lvlrates);
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
        public async Task<List<CustomerListModel>> SPCustomers(int id)
        {
            var query = $@"select distinct
                            c.Id as CustomerId,
                            c.BusinessName,
                            u.Name as RequestedBy,
                            cd.*,
                            format(cd.CreatedOn,'dd-MMM-yyyy HH:mm') as DateTime,
                            format(cd.UpdatedOn,'dd-MMM-yyyy HH:mm') as UpdatedDateTime,
                            cs.Name as StatusName,
                            u2.Name as [User],
                            c.IsFAC,
                            c.FuelFactor
                            from Customers c
                            inner join CustomerDetails cd on c.Id = cd.CustomerId
                            inner join ZNI_USER1 u on u.U_ID = c.CreatedBy
                            inner join ZNI_USER1 u2 on u2.U_ID = cd.UserId
                            inner join CustomerStatus cs on cs.Id = cd.Status
                            where c.CreatedBy = {id} and isnull(c.IsMatured,0) = 0;";

            var rs = (await con.QueryAsync<CustomerListModel>(query)).ToList();

            if (rs.Any())
            {
                var ids = string.Join(",", rs.Select(x => x.CustomerId).Distinct());

                query = $@"select pr.*,cp.Name as ProductName from CustomerProductRates pr inner join CustomerProducts cp on cp.Id = pr.ProductId where pr.CustomerId in({ids})";

                var rates = await con.QueryAsync<Rate>(query);

                var list = rs.GroupBy(x => x.CustomerId).Select(x => new CustomerListModel
                {
                    CustomerId = x.Key,
                    Remarks = "",
                    BusinessName = x.FirstOrDefault()?.BusinessName,
                    RequestedBy = x.FirstOrDefault()?.RequestedBy,
                    Approve = false,
                    Print = x.Any(z => z.IsLast && z.IsActive && z.UserStaffLevelId == CRFUserType.ZA),
                    Edit = x.Any(z => z.IsLast && z.UserId == id),
                    FinalStatus = x.Any(z => z.IsLast && z.UserStaffLevelId == CRFUserType.ZA && z.Status == 3),
                    NextApprover = x.FirstOrDefault(z => z.IsLast)?.User,
                    IsFAC = x.FirstOrDefault(z => z.IsLast).IsFAC,
                    FuelFactor = x.FirstOrDefault(z => z.IsLast).FuelFactor,
                    Details = x.OrderByDescending(c => c.Id).Select(y => new CustomerListDetailModel
                    {
                        Id = y.Id,
                        CreatedOn = y.CreatedOn,
                        User = y.User,
                        IsActive = y.IsActive,
                        IsLast = y.IsLast,
                        Remarks = y.Remarks,
                        Status = y.Status,
                        StatusName = y.StatusName,
                        UserId = y.UserId,
                        UserStaffLevelId = y.UserStaffLevelId,
                        DateTime = y.DateTime,
                        UpdatedDateTime = y.UpdatedDateTime
                    }),
                    Rates = rates.Where(z => z.CustomerId == x.Key)
                }).ToList();

                return list;
            }
            else
            {
                return new List<CustomerListModel>();
            }
        }
        public async Task<List<CustomerListModel>> AMCustomers(int id)
        {
            var query = $@"select distinct
                            c.Id as CustomerId,
                            c.BusinessName,
                            u.Name as RequestedBy,
                            cd.*,
                            format(cd.CreatedOn,'dd-MMM-yyyy HH:mm') as DateTime,
                            format(cd.UpdatedOn,'dd-MMM-yyyy HH:mm') as UpdatedDateTime,
                            cs.Name as StatusName,
                            u2.Name as [User],
                            c.Level,
                            c.IsFAC,
                            c.FuelFactor
                            from Customers c
                            inner join CustomerDetails cd on c.Id = cd.CustomerId
                            inner join ZNI_USER1 u on u.U_ID = c.CreatedBy
                            inner join ZNI_USER1 u2 on u2.U_ID = cd.UserId
                            inner join CustomerStatus cs on cs.Id = cd.Status
                            where c.Id in(select distinct CustomerId from CustomerDetails where UserStaffLevelId = 2 and UserId = {id} and IsActive = 1) and isnull(c.IsMatured,0) = 0;";

            var rs = (await con.QueryAsync<CustomerListModel>(query)).ToList();

            if (rs.Any())
            {
                var ids = string.Join(",", rs.Select(x => x.CustomerId).Distinct());

                query = $@"select pr.*,cp.Name as ProductName from CustomerProductRates pr inner join CustomerProducts cp on cp.Id = pr.ProductId where pr.CustomerId in({ids})";

                var rates = await con.QueryAsync<Rate>(query);

                var list = rs.GroupBy(x => x.CustomerId).Select(x => new CustomerListModel
                {
                    CustomerId = x.Key,
                    Remarks = "",
                    BusinessName = x.FirstOrDefault()?.BusinessName,
                    RequestedBy = x.FirstOrDefault()?.RequestedBy,
                    Approve = !x.Any(d => (d.IsLast && (int)d.UserStaffLevelId > 2 && d.IsActive) || (d.UserId == id && d.Status >= 3 && d.IsActive)),
                    Edit = false,
                    FinalStatus = x.Any(z => z.IsLast && z.UserStaffLevelId == CRFUserType.ZA && z.Status == 3),
                    NextApprover = x.FirstOrDefault(z => z.IsLast)?.User,
                    Level = x.FirstOrDefault().Level,
                    IsFAC = x.FirstOrDefault(z => z.IsLast).IsFAC,
                    FuelFactor = x.FirstOrDefault(z => z.IsLast).FuelFactor,
                    Details = x.OrderByDescending(c => c.Id).Select(y => new CustomerListDetailModel
                    {
                        Id = y.Id,
                        CreatedOn = y.CreatedOn,
                        User = y.User,
                        IsActive = y.IsActive,
                        IsLast = y.IsLast,
                        Remarks = y.Remarks,
                        Status = y.Status,
                        StatusName = y.StatusName,
                        UserId = y.UserId,
                        UserStaffLevelId = y.UserStaffLevelId,
                        DateTime = y.DateTime,
                        UpdatedDateTime = y.UpdatedDateTime
                    }),
                    Rates = rates.Where(z => z.CustomerId == x.Key)
                }).ToList();

                return list;
            }
            else
            {
                return new List<CustomerListModel>();
            }
        }
        public async Task<List<CustomerListModel>> GMCustomers(UserModel u)
        {
            var query = $@"with t as (
                                select distinct CustomerId from CustomerDetails where IsActive = 1 and UserStaffLevelId <= 2 and UserId in(
                                    select distinct u.U_ID from ZNI_USER1 u
                                    inner join UserStafflevels usl on u.U_ID = usl.UserId
                                    inner join (select distinct zoneCode from Zones
                                    where Region = (
                                    select distinct Region from Zones 
                                    where zoneCode = '{u.ZoneCode}') and zone_type = 1) t on t.zoneCode = u.ZoneCode
                                    where u.[STATUS] = 1 and usl.StaffLevelId = 2
                                    UNION
                                    select {u.Uid} as U_ID
                                )
                            )
                            select distinct
                            c.Id as CustomerId,
                            c.BusinessName,
                            u.Name as RequestedBy,
                            cd.*,
                            format(cd.CreatedOn,'dd-MMM-yyyy HH:mm') as DateTime,
                            format(cd.UpdatedOn,'dd-MMM-yyyy HH:mm') as UpdatedDateTime,
                            cs.Name as StatusName,
                            u2.Name as [User],
                            c.Level,
                            c.IsFAC,
                            c.FuelFactor
                            from Customers c
                            inner join CustomerDetails cd on c.Id = cd.CustomerId
                            inner join ZNI_USER1 u on u.U_ID = c.CreatedBy
                            inner join ZNI_USER1 u2 on u2.U_ID = cd.UserId
                            inner join CustomerStatus cs on cs.Id = cd.Status
                            where cd.IsActive = 1 and cd.CustomerId in (select * from t) and isnull(c.IsMatured,0) = 0;";

            var rs = (await con.QueryAsync<CustomerListModel>(query)).ToList();

            if (rs.Any())
            {
                var ids = string.Join(",", rs.Select(x => x.CustomerId).Distinct());

                query = $@"select pr.*,cp.Name as ProductName from CustomerProductRates pr inner join CustomerProducts cp on cp.Id = pr.ProductId where pr.CustomerId in({ids})";

                var rates = await con.QueryAsync<Rate>(query);

                var list = rs.GroupBy(x => x.CustomerId).Select(x => new CustomerListModel
                {
                    CustomerId = x.Key,
                    Remarks = "",
                    BusinessName = x.FirstOrDefault()?.BusinessName,
                    RequestedBy = x.FirstOrDefault()?.RequestedBy,
                    Approve = !x.Any(d => (d.IsLast && (int)d.UserStaffLevelId > 3 && d.IsActive) || (d.UserId == u.Uid && d.Status >= 3 && d.IsActive)),
                    Edit = false,
                    FinalStatus = x.Any(z => z.IsLast && z.UserStaffLevelId == CRFUserType.ZA && z.Status == 3),
                    NextApprover = x.FirstOrDefault(z => z.IsLast)?.User,
                    Level = x.FirstOrDefault().Level,
                    IsFAC = x.FirstOrDefault(z => z.IsLast).IsFAC,
                    FuelFactor = x.FirstOrDefault(z => z.IsLast).FuelFactor,
                    Details = x.OrderByDescending(c => c.Id).Select(y => new CustomerListDetailModel
                    {
                        Id = y.Id,
                        CreatedOn = y.CreatedOn,
                        User = y.User,
                        IsActive = y.IsActive,
                        IsLast = y.IsLast,
                        Remarks = y.Remarks,
                        Status = y.Status,
                        StatusName = y.StatusName,
                        UserId = y.UserId,
                        UserStaffLevelId = y.UserStaffLevelId,
                        DateTime = y.DateTime,
                        UpdatedDateTime = y.UpdatedDateTime
                    }),
                    Rates = rates.Where(z => z.CustomerId == x.Key)
                }).ToList();

                return list;
            }
            else
            {
                return new List<CustomerListModel>();
            }
        }
        public async Task<List<CustomerListModel>> DirectorCustomers(int id, UserModel u, List<int> productids)
        {
            string ids = string.Join(",", productids.Select(x => $"{x}"));
            var query = $@"select distinct
                            c.Id as CustomerId,
                            c.BusinessName,
                            u.Name as RequestedBy,
                            cd.*,
                            format(cd.CreatedOn,'dd-MMM-yyyy HH:mm') as DateTime,
                            format(cd.UpdatedOn,'dd-MMM-yyyy HH:mm') as UpdatedDateTime,
                            cs.Name as StatusName,
                            u2.Name as [User],
                            c.IsFAC,
                            c.FuelFactor
                            from Customers c
                            inner join CustomerProductRates cr on cr.CustomerId = c.Id
                            inner join CustomerDetails cd on c.Id = cd.CustomerId
                            inner join ZNI_USER1 u on u.U_ID = c.CreatedBy
                            inner join ZNI_USER1 u2 on u2.U_ID = cd.UserId
                            inner join CustomerStatus cs on cs.Id = cd.Status
                            where cd.IsActive = 1 and cr.ProductId in({ids}) and isnull(c.IsMatured,0) = 0;";

            var rs = (await con.QueryAsync<CustomerListModel>(query)).ToList();

            if (rs.Any())
            {
                ids = string.Join(",", rs.Select(x => x.CustomerId).Distinct());

                query = $@"select pr.*,cp.Name as ProductName from CustomerProductRates pr inner join CustomerProducts cp on cp.Id = pr.ProductId where pr.CustomerId in({ids})";

                var rates = await con.QueryAsync<Rate>(query);

                var list = rs.GroupBy(x => x.CustomerId).Select(x => new CustomerListModel
                {
                    CustomerId = x.Key,
                    Remarks = "",
                    BusinessName = x.FirstOrDefault()?.BusinessName,
                    RequestedBy = x.FirstOrDefault()?.RequestedBy,
                    Approve = !x.Any(d => (d.IsLast && (int)d.UserStaffLevelId > 4 && d.IsActive) || (d.UserId == u.Uid && d.Status >= 3 && d.IsActive)),
                    Edit = false,
                    FinalStatus = x.Any(z => z.IsLast && z.UserStaffLevelId == CRFUserType.ZA && z.Status == 3),
                    //NextApprover = x.Any(z => z.UserStaffLevelId == CRFUserType.Director && z.UserId == u.Uid && z.Status >= 3) ? x.FirstOrDefault(z => z.IsActive && z.us)?.User : u.Name,
                    NextApprover = x.Any(z => z.UserStaffLevelId != CRFUserType.Director && z.Status < 3 && z.IsLast && z.IsActive) ?
                    x.FirstOrDefault(z => z.UserStaffLevelId != CRFUserType.Director && z.Status < 3 && z.IsLast && z.IsActive)?.User :
                    x.Any(z => z.UserStaffLevelId == CRFUserType.Director && z.UserId == u.Uid && z.Status >= 3 && z.IsActive) ?
                    x.FirstOrDefault(z => z.UserStaffLevelId == CRFUserType.Director && z.Status < 3 && z.IsActive).User
                    : u.UserName,
                    IsFAC = x.FirstOrDefault(z => z.IsLast).IsFAC,
                    FuelFactor = x.FirstOrDefault(z => z.IsLast).FuelFactor,
                    Details = x.OrderByDescending(c => c.Id).Select(y => new CustomerListDetailModel
                    {
                        Id = y.Id,
                        CreatedOn = y.CreatedOn,
                        User = y.User,
                        IsActive = y.IsActive,
                        IsLast = y.IsLast,
                        Remarks = y.Remarks,
                        Status = y.Status,
                        StatusName = y.StatusName,
                        UserId = y.UserId,
                        UserStaffLevelId = y.UserStaffLevelId,
                        DateTime = y.DateTime,
                        UpdatedDateTime = y.UpdatedDateTime
                    }),
                    Rates = rates.Where(z => z.CustomerId == x.Key)
                }).ToList();

                return list;
            }
            else
            {
                return new List<CustomerListModel>();
            }
        }
        public async Task<List<CustomerListModel>> ZACustomers(int id)
        {
            var query = $@"select distinct
                            c.Id as CustomerId,
                            c.BusinessName,
                            u.Name as RequestedBy,
                            u.branchcode as BranchCode,
                            cd.*,
                            format(cd.CreatedOn,'dd-MMM-yyyy HH:mm') as DateTime,
                            format(cd.UpdatedOn,'dd-MMM-yyyy HH:mm') as UpdatedDateTime,
                            cs.Name as StatusName,
                            u2.Name as [User],
                            c.IsFAC,
                            c.FuelFactor
                            from Customers c
                            inner join CustomerDetails cd on c.Id = cd.CustomerId
                            inner join ZNI_USER1 u on u.U_ID = c.CreatedBy
                            inner join ZNI_USER1 u2 on u2.U_ID = cd.UserId
                            inner join CustomerStatus cs on cs.Id = cd.Status
                            where c.Id in(select distinct CustomerId from CustomerDetails where UserStaffLevelId = 5 and UserId = {id} and IsActive = 1 and IsLast = 1) and isnull(c.IsMatured,0) = 0;";

            var rs = (await con.QueryAsync<CustomerListModel>(query)).ToList();

            if (rs.Any())
            {
                var ids = string.Join(",", rs.Select(x => x.CustomerId).Distinct());

                query = $@"select pr.*,cp.Name as ProductName from CustomerProductRates pr inner join CustomerProducts cp on cp.Id = pr.ProductId where pr.CustomerId in({ids})";

                var rates = await con.QueryAsync<Rate>(query);

                var list = rs.GroupBy(x => x.CustomerId).Select(x => new CustomerListModel
                {
                    CustomerId = x.Key,
                    Remarks = "",
                    BusinessName = x.FirstOrDefault()?.BusinessName,
                    RequestedBy = x.FirstOrDefault()?.RequestedBy,
                    Approve = !x.Any(d => (int)d.UserStaffLevelId == 5 && d.Status >= 3),
                    Edit = false,
                    Print = false,
                    FinalStatus = x.Any(z => z.IsLast && z.UserStaffLevelId == CRFUserType.ZA && z.Status == 3),
                    NextApprover = x.FirstOrDefault(z => z.IsLast)?.User,
                    IsFAC = x.FirstOrDefault(z => z.IsLast).IsFAC,
                    FuelFactor = x.FirstOrDefault(z => z.IsLast).FuelFactor,
                    BranchCode = x.FirstOrDefault().BranchCode,
                    Details = x.OrderByDescending(c => c.Id).Select(y => new CustomerListDetailModel
                    {
                        Id = y.Id,
                        CreatedOn = y.CreatedOn,
                        User = y.User,
                        IsActive = y.IsActive,
                        IsLast = y.IsLast,
                        Remarks = y.Remarks,
                        Status = y.Status,
                        StatusName = y.StatusName,
                        UserId = y.UserId,
                        UserStaffLevelId = y.UserStaffLevelId,
                        DateTime = y.DateTime,
                        UpdatedDateTime = y.UpdatedDateTime
                    }),
                    Rates = rates.Where(z => z.CustomerId == x.Key)
                }).ToList();
                return list;
            }
            else
            {
                return new List<CustomerListModel>();
            }
        }
        public async Task<(bool success, string msg)> AMApproveReject(bool approve, int id, string remarks, UserModel u, int customerLevel)
        {
            SqlTransaction trans = null;
            try
            {
                string msg = "Customer Status Updated";
                bool success = true;
                var rs = 0;
                await con.OpenAsync();
                trans = con.BeginTransaction();
                if (approve)
                {
                    var query = customerLevel <= 2 ?
                        $@"select distinct usl.UserId from Customers c
                            inner join ZNI_USER1 u on c.CreatedBy = u.U_ID
                            inner join ZNI_USER1 u2 on u2.ZoneCode = u.ZoneCode
                            inner join UserStaffLevels usl on usl.UserId = u2.U_ID
                            inner join MNP_StaffLevel sl on sl.Id = usl.StaffLevelId
                            where c.Id = {id} and sl.[Level] = 5;" :
                        $@"select distinct u.U_ID from ZNI_USER1 u
                            inner join UserStaffLevels usl on u.U_ID = usl.UserId
                            inner join MNP_StaffLevel sl on sl.Id = usl.StaffLevelId
                            where u.ZoneCode in(
                            select distinct zoneCode from Zones where Region in(select distinct Region from zones z where z.ZoneCode = '{u.ZoneCode}')
                            ) and sl.[Level] = 3;";

                    var nextApproverId = await con.QueryFirstOrDefaultAsync<int>(query, commandTimeout: int.MaxValue, transaction: trans);
                    if (nextApproverId > 0)
                    {
                        query = $@"update CustomerDetails set [Status] = 3, UpdatedOn = getdate(),Remarks = '{remarks}', IsLast = 0 where CustomerId = {id} and UserStaffLevelId = 2 and UserId = {u.Uid} and IsLast = 1;
                                INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) VALUES
                                ({id},{nextApproverId},{(customerLevel <= 2 ? 5 : 3)},2,1);";
                        rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);
                    }
                    else
                    {
                        msg = "Next approver not found";
                        success = false;
                    }
                }
                else
                {
                    var query = $@"update CustomerDetails set [Status] = 4, UpdatedOn = getdate(),Remarks = '{remarks}', IsLast = 0 where CustomerId = {id} and UserStaffLevelId = 2 and UserId = {u.Uid} and IsLast = 1;
                                INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) VALUES
                                ({id},(select top 1 CreatedBy from Customers where Id = {id}),1,1,1);
                                update CustomerDetails set IsActive = 0 where CustomerId = {id}";
                    rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);
                }

                trans.Commit();
                con.Close();

                return (success, msg);
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
        public async Task<(bool success, string msg)> GMApproveReject(bool approve, int id, string remarks, UserModel u, int customerLevel)
        {
            SqlTransaction trans = null;
            try
            {
                string msg = "Customer Status Updated";
                bool success = true;
                var rs = 0;
                await con.OpenAsync();
                trans = con.BeginTransaction();
                if (approve)
                {
                    var query = customerLevel <= 3 ?
                        $@"select distinct usl.UserId from Customers c
                            inner join ZNI_USER1 u on c.CreatedBy = u.U_ID
                            inner join ZNI_USER1 u2 on u2.ZoneCode = u.ZoneCode
                            inner join UserStaffLevels usl on usl.UserId = u2.U_ID
                            inner join MNP_StaffLevel sl on sl.Id = usl.StaffLevelId
                            where c.Id = {id} and sl.[Level] = 5;" :
                            $@"select distinct usl.UserId from CustomerProductRates cr
                            inner join MNP_StaffLevel sl on cr.ProductId = sl.ProductId
                            inner join UserStaffLevels usl on usl.StaffLevelId = sl.Id
                            where CustomerId = {id};";

                    var nextApproverIds = await con.QueryAsync<int>(query, commandTimeout: int.MaxValue, transaction: trans);
                    if (nextApproverIds.Any())
                    {
                        query = $@"select count(*) from CustomerDetails where CustomerId = {id} and UserStaffLevelId = 3 and UserId = {u.Uid} and [Status] < 3 and IsActive = 1 and IsLast = 1;";
                        var entryExist = await con.QueryFirstOrDefaultAsync<int>(query, commandTimeout: int.MaxValue, transaction: trans);

                        if (entryExist == 1)
                            query = $@"update CustomerDetails set [Status] = 3, UpdatedOn = getdate(),Remarks = '{remarks}', IsLast = 0 where CustomerId = {id} and UserStaffLevelId = 3 and UserId = {u.Uid} and IsLast = 1 and IsActive = 1;";
                        else
                            query = $@"update CustomerDetails set IsLast = 0 where CustomerId = {id};
                                       INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast,Remarks,UpdatedOn) VALUES({id},{u.Uid},3,3,0,'{remarks}',getdate());";

                        for (int i = 0; i < nextApproverIds.Count(); i++)
                        {
                            if (i == nextApproverIds.Count() - 1)
                                query += $@"INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) VALUES({id},{nextApproverIds.ElementAt(i)},{(customerLevel <= 3 ? 5 : 4)},2,1);";
                            else
                                query += $@"INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) VALUES({id},{nextApproverIds.ElementAt(i)},4,2,0);";
                        }
                        rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);
                    }
                    else
                    {
                        msg = "Next approver not found";
                        success = false;
                    }
                }
                else
                {
                    var query = $@"select count(*) from CustomerDetails where CustomerId = {id} and UserStaffLevelId = 3 and UserId = {u.Uid} and [Status] < 3 and IsActive = 1 and IsLast = 1;";
                    var entryExist = await con.QueryFirstOrDefaultAsync<int>(query, commandTimeout: int.MaxValue, transaction: trans);

                    if (entryExist == 1)
                        query = $@"update CustomerDetails set [Status] = 4, UpdatedOn = getdate(),Remarks = '{remarks}', IsLast = 0 where CustomerId = {id} and UserStaffLevelId = 3 and UserId = {u.Uid} and IsLast = 1 and IsActive = 1;";
                    else
                        query = $@"update CustomerDetails set IsLast = 0 where CustomerId = {id};
                                    INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast,Remarks,UpdatedOn) VALUES({id},{u.Uid},3,4,0,'{remarks}',getdate());";


                    query += $@"INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) VALUES
                                ({id},(select top 1 CreatedBy from Customers where Id = {id}),1,1,1);
                                update CustomerDetails set IsActive = 0 where CustomerId = {id}";
                    rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);
                }

                trans.Commit();
                con.Close();

                return (success, msg);
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
        //public async Task<(bool success, string msg)> DRApproveReject(bool approve, int id, string remarks, UserModel u)
        //{
        //    SqlTransaction trans = null;
        //    try
        //    {
        //        string msg = "Customer Status Updated";
        //        bool success = true;
        //        var rs = 0;
        //        await con.OpenAsync();
        //        trans = con.BeginTransaction();
        //        if (approve)
        //        {
        //            var query = $@"select distinct usl.UserId from CustomerProductRates cr
        //                        inner join MNP_StaffLevel sl on cr.ProductId = sl.ProductId
        //                        inner join UserStaffLevels usl on usl.StaffLevelId = sl.Id
        //                        left join CustomerDetails cd on cd.CustomerId = cr.CustomerId and cd.UserStaffLevelId = 4 and cd.IsActive = 1
        //                        where cr.CustomerId = {id} and cd.UserId is null;";

        //            var nextApproverIds = await con.QueryAsync<int>(query, commandTimeout: int.MaxValue, transaction: trans);

        //            if (nextApproverIds.Any())
        //            {
        //                query = $@"update CustomerDetails set IsLast = 0 where CustomerId = {id};";
        //                for (int i = 0; i < nextApproverIds.Count(); i++)
        //                {
        //                    if (i == nextApproverIds.Count() - 1)
        //                        query += $@"INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) VALUES({id},{nextApproverIds.ElementAt(i)},4,2,1);";
        //                    else
        //                        query += $@"INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) VALUES({id},{nextApproverIds.ElementAt(i)},4,2,0);";
        //                }
        //                rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);

        //            }

        //            query = $@"update CustomerDetails set [Status] = 3, UpdatedOn = getdate(),Remarks = '{remarks}' where CustomerId = {id} and UserStaffLevelId = 4 and UserId = {u.Uid} and IsActive = 1;";
        //            rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);

        //            query = $@"select 
        //                    (select count(distinct ProductId) from CustomerProductRates where CustomerId = {id}) as Total,
        //                    (select count(*) from CustomerDetails cd
        //                    inner join UserStaffLevels usl on usl.UserId = cd.UserId
        //                    inner join MNP_StaffLevel sl on sl.id = usl.StaffLevelId
        //                    where cd.CustomerId = {id} and cd.UserStaffLevelId = 4 and cd.[Status] = 3 and sl.[Level] = 4) as TotalApproved,
        //                    (select distinct usl.UserId from Customers c
        //                    inner join ZNI_USER1 u on c.CreatedBy = u.U_ID
        //                    inner join ZNI_USER1 u2 on u2.ZoneCode = u.ZoneCode
        //                    inner join UserStaffLevels usl on usl.UserId = u2.U_ID
        //                    inner join MNP_StaffLevel sl on sl.Id = usl.StaffLevelId
        //                    where c.Id = {id} and sl.[Level] = 5) as ZA;";

        //            var totalDirectorsAndZA = await con.QueryFirstOrDefaultAsync<(int Total, int TotalApproved, int ZA)>(query, commandTimeout: int.MaxValue, transaction: trans);

        //            if (totalDirectorsAndZA.TotalApproved == totalDirectorsAndZA.Total)
        //            {
        //                query = $@"update CustomerDetails set IsLast = 0 where CustomerId = {id};
        //                            INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) VALUES({id},{totalDirectorsAndZA.ZA},5,2,1);";
        //                await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);
        //            }
        //        }
        //        else
        //        {
        //            var query = $@"select count(*) from CustomerDetails where CustomerId = {id} and UserStaffLevelId = 4 and UserId = {u.Uid} and [Status] < 3 and IsActive = 1;";
        //            var entryExist = await con.QueryFirstOrDefaultAsync<int>(query, commandTimeout: int.MaxValue, transaction: trans);

        //            if (entryExist == 1)
        //                query = $@"update CustomerDetails set [Status] = 4, UpdatedOn = getdate(),Remarks = '{remarks}', IsLast = 0 where CustomerId = {id} and UserStaffLevelId = 4 and UserId = {u.Uid} and IsActive = 1;";
        //            else
        //                query = $@"update CustomerDetails set IsLast = 0 where CustomerId = {id};
        //                        INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast,Remarks,UpdatedOn) VALUES({id},{u.Uid},4,4,0,'{remarks}',getdate());";

        //            query += $@"INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) VALUES
        //                        ({id},(select top 1 CreatedBy from Customers where Id = {id}),1,1,1);
        //                        update CustomerDetails set IsActive = 0 where CustomerId = {id}";
        //            rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);
        //        }

        //        trans.Commit();
        //        con.Close();

        //        return (success, msg);
        //    }
        //    catch (SqlException ex)
        //    {
        //        if (con.State == System.Data.ConnectionState.Open)
        //        {
        //            trans.Rollback();
        //            con.Close();
        //        }
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State == System.Data.ConnectionState.Open)
        //        {
        //            trans.Rollback();
        //            con.Close();
        //        }
        //        throw ex;
        //    }
        //}
        public async Task<(bool success, string msg)> DRApproveReject(bool approve, int id, string remarks, UserModel u)
        {
            SqlTransaction trans = null;
            try
            {
                string msg = "Customer Status Updated";
                bool success = true;
                var rs = 0;
                await con.OpenAsync();
                trans = con.BeginTransaction();
                if (approve)
                {
                    var query = $@"select distinct usl.UserId from CustomerProductRates cr
                                inner join MNP_StaffLevel sl on cr.ProductId = sl.ProductId
                                inner join UserStaffLevels usl on usl.StaffLevelId = sl.Id
                                left join CustomerDetails cd on cd.CustomerId = cr.CustomerId and cd.UserStaffLevelId = 4 and cd.IsActive = 1
                                where cr.CustomerId = {id} and cd.UserId is null;";

                    var nextApproverIds = await con.QueryAsync<int>(query, commandTimeout: int.MaxValue, transaction: trans);

                    if (nextApproverIds.Any())
                    {
                        query = $@"update CustomerDetails set IsLast = 0 where CustomerId = {id};";
                        for (int i = 0; i < nextApproverIds.Count(); i++)
                        {
                            if (i == nextApproverIds.Count() - 1)
                                query += $@"INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) VALUES({id},{nextApproverIds.ElementAt(i)},4,2,1);";
                            else
                                query += $@"INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) VALUES({id},{nextApproverIds.ElementAt(i)},4,2,0);";
                        }
                        rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);
                    }

                    query = $@"update CustomerDetails set [Status] = 3, UpdatedOn = getdate(),Remarks = '{remarks}' where CustomerId = {id} and UserStaffLevelId = 4 and UserId = {u.Uid} and IsActive = 1;";
                    rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);

                    query = $@"select 
                            (select count(distinct ProductId) from CustomerProductRates where CustomerId = {id}) as Total,
                            (select count(*) from CustomerDetails cd
                            inner join UserStaffLevels usl on usl.UserId = cd.UserId
                            inner join MNP_StaffLevel sl on sl.id = usl.StaffLevelId
                            where cd.CustomerId = {id} and cd.UserStaffLevelId = 4 and cd.[Status] = 3 and sl.[Level] = 4) as TotalApproved,
                            (select distinct usl.UserId from Customers c
                            inner join ZNI_USER1 u on c.CreatedBy = u.U_ID
                            inner join ZNI_USER1 u2 on u2.ZoneCode = u.ZoneCode
                            inner join UserStaffLevels usl on usl.UserId = u2.U_ID
                            inner join MNP_StaffLevel sl on sl.Id = usl.StaffLevelId
                            where c.Id = {id} and sl.[Level] = 5) as ZA;";

                    var totalDirectorsAndZA = await con.QueryFirstOrDefaultAsync<(int Total, int TotalApproved, int ZA)>(query, commandTimeout: int.MaxValue, transaction: trans);

                    query = $@"update CustomerDetails set IsLast = 0 where CustomerId = {id};
                                    INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) VALUES({id},{totalDirectorsAndZA.ZA},5,2,1);";
                    await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);
                    //if (totalDirectorsAndZA.TotalApproved == totalDirectorsAndZA.Total)
                    //{
                    //}
                }
                else
                {
                    var query = $@"select count(*) from CustomerDetails where CustomerId = {id} and UserStaffLevelId = 4 and UserId = {u.Uid} and [Status] < 3 and IsActive = 1;";
                    var entryExist = await con.QueryFirstOrDefaultAsync<int>(query, commandTimeout: int.MaxValue, transaction: trans);

                    if (entryExist == 1)
                        query = $@"update CustomerDetails set [Status] = 4, UpdatedOn = getdate(),Remarks = '{remarks}', IsLast = 0 where CustomerId = {id} and UserStaffLevelId = 4 and UserId = {u.Uid} and IsActive = 1;";
                    else
                        query = $@"update CustomerDetails set IsLast = 0 where CustomerId = {id};
                                INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast,Remarks,UpdatedOn) VALUES({id},{u.Uid},4,4,0,'{remarks}',getdate());";

                    query += $@"INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) VALUES
                                ({id},(select top 1 CreatedBy from Customers where Id = {id}),1,1,1);
                                update CustomerDetails set IsActive = 0 where CustomerId = {id}";
                    rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);
                }

                trans.Commit();
                con.Close();

                return (success, msg);
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
        public async Task<(bool success, string msg)> ZAApproveReject(bool approve, int id, string remarks, UserModel u)
        {
            SqlTransaction trans = null;
            try
            {
                string msg = "";
                bool success = false;
                var rs = 0;
                await con.OpenAsync();
                trans = con.BeginTransaction();
                if (approve)
                {
                    var query = $@"select c.*, pc.Name as City, 
                                    case when (select count(*) from CustomerProductRates where CustomerId = c.Id and ProductId = 2) > 0 then 1 else 0 end as IsCod,
                                    u.branchcode as BranchCode,
                                    u.ZoneCode
                                    from Customers c
                                    inner join ZNI_USER1 u on u.U_ID = c.CreatedBy
                                    inner join PakCities pc on c.CityId = pc.Id
                                    where c.Id = {id};";

                    var customer = await con.QueryFirstOrDefaultAsync(query, transaction: trans);

                    query = $@"declare @accountNo NVARCHAR(10) = (select concat('{customer.BranchCode}','{customer.BusinessName.Substring(0, 1).ToUpper()}', (count(*) + 1)) as AccountNo from CreditClients where branchCode = '{customer.BranchCode}' and [status] = 1);
                                if not exists(select * from CreditClients where accountNo = @accountNo)
                                BEGIN
                                select @accountNo as AccountNo, 1 as Success;
                                end
                                ELSE
                                BEGIN
                                select @accountNo as AccountNo, 0 as Success;
                                end;";

                    var accountinfo = await con.QueryFirstOrDefaultAsync<(string AccountNo, bool Success)>(query, transaction: trans);

                    if (accountinfo.Success)
                    {
                        query = $@"update CustomerDetails set [Status] = 3, UpdatedOn = getdate(),Remarks = '{remarks}' where CustomerId = {id} and UserStaffLevelId = 5 and UserId = {u.Uid} and IsLast = 1;
                                    update Customers set AccountNo = '{accountinfo.AccountNo}', IsMatured = 1 where Id = {id};";
                        rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);

                        query = $@"insert into CreditClients([name],[contactPerson],[phoneNo],[faxNo],[email],[address],[centralizedClient],[regDate],[regEndDate],[status],[printingStatus],[clientGrpId],[createdBy],[createdOn],[IndustryId],[accountNo],[zoneCode],[branchCode],[expressCenterCode],[ntnNo],[IsCOD],[isActive],[isFranchisee],[MailingAddress],[OriginEC],[IsSmsServiceActive],[BeneficiaryName],[BeneficiaryBankCode],[Faftype],[isWebtracking],[ContactPersonDesignation],[isMinBilling],[AllowSMS],[rtntype],[rtnbranch])
                                    values(
                                    '{customer.BusinessName}',
                                    '{customer.ContactPerson}',
                                    '{customer.PhoneNo}',
                                    '{customer.FaxNo}',
                                    '{customer.Email}',
                                    '{customer.HouseOrOfficeNo} {customer.FloorNo} {customer.BuildingName} {customer.PlotNo} {customer.Street} {customer.SectorOrZone} {customer.Area} {customer.City}',
                                    0,
                                    getdate(),
                                    dateadd(year, 1, getdate()),
                                    1,
                                    {(customer.PrintInvoice ? 1 : 0)},
                                    {(customer.GroupId != null ? customer.GroupId : 0)},
                                    {u.Uid},
                                    getdate(),
                                    '{customer.IndustryId}',
                                    '{accountinfo.AccountNo}',
                                    '{customer.ZoneCode}',
                                    '{customer.BranchCode}',
                                    (select top 1 expressCenterCode from ExpressCenters where ISNULL(Main_EC,0) = 1 and bid = {customer.BranchCode} and [status] = 1),
                                    '{customer.NTNNumber}',
                                    {(customer.IsCod == 1 ? 1 : 0)},
                                    1,
                                    0,
                                    '{customer.HouseOrOfficeNo} {customer.FloorNo} {customer.BuildingName} {customer.PlotNo} {customer.Street} {customer.SectorOrZone} {customer.Area} {customer.City}',
                                    (select top 1 expressCenterCode from ExpressCenters where ISNULL(Main_EC,0) = 1 and bid = {customer.BranchCode} and [status] = 1),
                                    0,
                                    '{customer.BeneficiaryName}',
                                    '{customer.BankBranchCode}',
                                    {(customer.IsFAF ? 1 : 0)},
                                    1,
                                    '{customer.Designation}',
                                    0,
                                    0,
                                    0,
                                    '{customer.BranchCode}');
                                    select SCOPE_IDENTITY() as Id;";

                        rs = await con.QueryFirstOrDefaultAsync<int>(query, commandTimeout: int.MaxValue, transaction: trans);

                        query = $@"insert into tempClientTariff values ({rs}, 0, 'overnight', '4','2', '2', 0.5, 1, 100, 1, 0, 0, 0, 0, 0, null, getdate(), null, {u.Uid}, null);";
                        await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);

                        trans.Commit();

                        if (customer.IsCod == 1)
                        {
                            string user = customer.ContactPerson.Split(' ')[0] + "_" + accountinfo.AccountNo;
                            await codcon.OpenAsync();
                            var z = await codcon.QueryFirstOrDefaultAsync<string>("COD_CREATE_SINGLE_LOC_USER", new { @userID = user, @AccNo = accountinfo.AccountNo }, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: int.MaxValue);
                            codcon.Close();
                            if (z == "Created Successfully On Cloud")
                            {
                                z = await con.QueryFirstOrDefaultAsync<string>("SP_CLOUD_COD_USERCREATION_PART2", new { @userID = user, @AccNo = accountinfo.AccountNo }, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: int.MaxValue);
                                if (z == "Created Successfully On 36")
                                {
                                    var subject = $"Shipper Name: {customer.ContactPerson} & Account No: {accountinfo.AccountNo}";
                                    var body = $@"Dear {customer.ContactPerson},<br><br>
                                                    Username: {user}<br>
                                                    Password: 12345<br>
                                                    URL: https://mnpcourier.com/mycod<br>
                                                    <br><br><br> <i>This is a system generated email and doesn’t require any signature. Please do not reply on this email.</i>";

                                    Email.SendEmail(new List<string> { customer.Email }, null, subject, body);
                                }
                            }
                        }
                        else
                        {
                            Email.SendEmail(new List<string> { customer.Email }, null, "Account Info", $"Your Account# is {accountinfo.AccountNo}");
                        }

                        success = true;
                        msg = $"Account# {accountinfo.AccountNo} Generated Successfully";
                    }
                    else
                    {
                        trans.Commit();
                        success = false;
                        msg = $"Account# {accountinfo.AccountNo} Already Exists";
                    }
                }
                else
                {
                    var query = $@"update CustomerDetails set [Status] = 4, UpdatedOn = getdate(),Remarks = '{remarks}', IsLast = 0 where CustomerId = {id} and UserStaffLevelId = 5 and UserId = {u.Uid} and IsLast = 1;
                                INSERT into CustomerDetails (CustomerId,UserId,UserStaffLevelId,[Status],IsLast) VALUES
                                ({id},(select top 1 CreatedBy from Customers where Id = {id}),1,1,1);
                                update CustomerDetails set IsActive = 0 where CustomerId = {id}";
                    rs = await con.ExecuteAsync(query, commandTimeout: int.MaxValue, transaction: trans);
                    trans.Commit();
                }

                con.Close();

                return (success, msg);
            }
            catch (SqlException ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    if (trans.Connection != null)
                        trans.Rollback();
                    con.Close();
                }
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    if (trans.Connection != null)
                        trans.Rollback();
                    con.Close();
                }
                throw ex;
            }
        }
        public async Task<(CRFUserType customerlevel, CRFUserType detaillevel)> GetUserRoles(SqlTransaction trans, Customer c)
        {
            var query = $@"select * from ProductRates;";
            var rs = await con.QueryAsync<Rate>(query, transaction: trans);
            var lvlrates = rs.Where(x => x.Level == 1);
            var alltrue = AllTrueInLevel(lvlrates, c);

            if (alltrue)
            {
                return (CRFUserType.SalesPerson, CRFUserType.ZA);
            }
            else
            {
                lvlrates = rs.Where(x => x.Level == 2);
                alltrue = AllTrueInLevel(lvlrates, c);
                if (alltrue)
                {
                    return (CRFUserType.AreaManager, CRFUserType.AreaManager);
                }
                else
                {
                    lvlrates = rs.Where(x => x.Level == 3);
                    alltrue = AllTrueInLevel(lvlrates, c);
                    if (alltrue)
                    {
                        return (CRFUserType.GeneralManager, CRFUserType.AreaManager);
                    }
                    else
                    {
                        return (CRFUserType.Director, CRFUserType.AreaManager);
                    }
                }
            }
        }
        public bool AllTrueInLevel(IEnumerable<Rate> lvlrates, Customer c)
        {
            bool alltrue = false;

            if (c.IsFAC)
            {
                if (c.FuelFactor >= lvlrates.FirstOrDefault().FACFuelFactor)
                {
                    alltrue = true;
                }
                else
                {
                    return false;
                }
            }

            foreach (var item in lvlrates)
            {
                var rate = c.Rates.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (rate != null)
                {

                    if (rate.ProductId == 1 || rate.ProductId == 2)
                    {
                        if (rate.Overnight && rate.SecondDay)
                        {
                            if (
                                rate.ZeroToPoint5KGWC >= item.ZeroToPoint5KGWC &&
                                rate.ZeroToPoint5KGDZ >= item.ZeroToPoint5KGDZ &&
                                rate.ZeroToPoint5KGSZ >= item.ZeroToPoint5KGSZ &&
                                rate.Point5To1KGWC >= item.Point5To1KGWC &&
                                rate.Point5To1KGDZ >= item.Point5To1KGDZ &&
                                rate.Point5To1KGSZ >= item.Point5To1KGSZ &&
                                ((rate.AddFactor == (decimal)0.5 && rate.AddKGWC >= item.AddKGWC) || (rate.AddFactor == (decimal)1 && rate.AddKGWC >= item.AddKGWC2)) &&
                                ((rate.AddFactor == (decimal)0.5 && rate.AddKGSZ >= item.AddKGSZ) || (rate.AddFactor == (decimal)1 && rate.AddKGSZ >= item.AddKGSZ2)) &&
                                ((rate.AddFactor == (decimal)0.5 && rate.AddKGDZ >= item.AddKGDZ) || (rate.AddFactor == (decimal)1 && rate.AddKGDZ >= item.AddKGDZ2)) &&
                                ((rate.MinWeight == (decimal)3 && rate.SecDayMin >= item.SecDayMin) || (rate.MinWeight == (decimal)5 && rate.SecDayMin >= item.SecDayMin2)) &&
                                ((rate.MinWeight == (decimal)3 && rate.SecDayAdd >= item.SecDayAdd) || (rate.MinWeight == (decimal)5 && rate.SecDayAdd >= item.SecDayAdd2))
                                )
                            {
                                alltrue = true;
                            }
                            else
                            {
                                alltrue = false;
                                break;
                            }
                        }
                        else if (rate.Overnight)
                        {
                            if (rate.ZeroToPoint5KGWC >= item.ZeroToPoint5KGWC &&
                                rate.ZeroToPoint5KGDZ >= item.ZeroToPoint5KGDZ &&
                                rate.ZeroToPoint5KGSZ >= item.ZeroToPoint5KGSZ &&
                                rate.Point5To1KGWC >= item.Point5To1KGWC &&
                                rate.Point5To1KGDZ >= item.Point5To1KGDZ &&
                                rate.Point5To1KGSZ >= item.Point5To1KGSZ &&
                                ((rate.AddFactor == (decimal)0.5 && rate.AddKGWC >= item.AddKGWC) || (rate.AddFactor == (decimal)1 && rate.AddKGWC >= item.AddKGWC2)) &&
                                ((rate.AddFactor == (decimal)0.5 && rate.AddKGSZ >= item.AddKGSZ) || (rate.AddFactor == (decimal)1 && rate.AddKGSZ >= item.AddKGSZ2)) &&
                                ((rate.AddFactor == (decimal)0.5 && rate.AddKGDZ >= item.AddKGDZ) || (rate.AddFactor == (decimal)1 && rate.AddKGDZ >= item.AddKGDZ2))
                              )
                            {
                                alltrue = true;
                            }
                            else
                            {
                                alltrue = false;
                                break;
                            }
                        }
                        else if (rate.SecondDay)
                        {
                            if (((rate.MinWeight == (decimal)3 && rate.SecDayMin >= item.SecDayMin) || (rate.MinWeight == (decimal)5 && rate.SecDayMin >= item.SecDayMin2)) &&
                                ((rate.MinWeight == (decimal)3 && rate.SecDayAdd >= item.SecDayAdd) || (rate.MinWeight == (decimal)5 && rate.SecDayAdd >= item.SecDayAdd2))
                               )
                            {
                                alltrue = true;
                            }
                            else
                            {
                                alltrue = false;
                                break;
                            }
                        }
                    }
                    else if (rate.ProductId == 3)
                    {
                        if (rate.ECargo)
                        {
                            if (
                               (rate.MinWeight == (decimal)3 && rate.ZoneAAdd >= item.ZoneAAdd) || (rate.MinWeight == (decimal)5 && rate.ZoneAAdd >= item.ZoneAAdd2) &&
                               (rate.MinWeight == (decimal)3 && rate.ZoneAMin >= item.ZoneAMin) || (rate.MinWeight == (decimal)5 && rate.ZoneAMin >= item.ZoneAMin2) &&
                               (rate.MinWeight == (decimal)3 && rate.ZoneBAdd >= item.ZoneBAdd) || (rate.MinWeight == (decimal)5 && rate.ZoneBAdd >= item.ZoneBAdd2) &&
                               (rate.MinWeight == (decimal)3 && rate.ZoneBMin >= item.ZoneBMin) || (rate.MinWeight == (decimal)5 && rate.ZoneBMin >= item.ZoneBMin2)
                               )
                            {
                                alltrue = true;
                            }
                            else
                            {
                                alltrue = false;
                                break;
                            }
                        }
                    }
                    else if (rate.ProductId == 5)
                    {
                        if (rate.Flyer && rate.FlyerWind)
                        {
                            if (
                                rate.FlyerS >= item.FlyerS &&
                                rate.FlyerM >= item.FlyerM &&
                                rate.FlyerL >= item.FlyerL &&
                                rate.FlyerXL >= item.FlyerXL &&
                                rate.FlyerWinS >= item.FlyerWinS &&
                                rate.FlyerWinM >= item.FlyerWinM &&
                                rate.FlyerWinL >= item.FlyerWinL &&
                                rate.FlyerWinXL >= item.FlyerWinXL
                                )
                            {
                                alltrue = true;
                            }
                            else
                            {
                                alltrue = false;
                                break;
                            }
                        }
                        else if (rate.Flyer)
                        {
                            if (
                                rate.FlyerS >= item.FlyerS &&
                                rate.FlyerM >= item.FlyerM &&
                                rate.FlyerL >= item.FlyerL &&
                                rate.FlyerXL >= item.FlyerXL
                                )
                            {
                                alltrue = true;
                            }
                            else
                            {
                                alltrue = false;
                                break;
                            }
                        }
                        else if (rate.FlyerWind)
                        {
                            if (
                                rate.FlyerWinS >= item.FlyerWinS &&
                                rate.FlyerWinM >= item.FlyerWinM &&
                                rate.FlyerWinL >= item.FlyerWinL &&
                                rate.FlyerWinXL >= item.FlyerWinXL
                                )
                            {
                                alltrue = true;
                            }
                            else
                            {
                                alltrue = false;
                                break;
                            }
                        }
                    }
                }
            }

            return alltrue;
        }
        //public async Task<(bool success, string msg)> GenerateAccountNo(int CustomerId, int RateId, string Name, string Branch)
        //{
        //    bool s = false;
        //    string m = "";
        //    SqlTransaction trans = null;
        //    try
        //    {
        //        var query = $@"declare @accountNo NVARCHAR(10) = (select concat('{Branch}','{Name.Substring(0, 1).ToUpper()}', (count(*) + 1)) as AccountNo from CreditClients where branchCode = '{Branch}' and [status] = 1);
        //                        if not exists(select * from CreditClients where accountNo = @accountNo)
        //                        BEGIN
        //                        select @accountNo as AccountNo;
        //                        end
        //                        ELSE
        //                        BEGIN
        //                        select null AccountNo;
        //                        end;";

        //        await con.OpenAsync();
        //        trans = con.BeginTransaction();

        //        var accountNo = await con.QueryFirstOrDefaultAsync<string>(query, transaction: trans);

        //        if (!string.IsNullOrEmpty(accountNo))
        //        {
        //            query = $@"update CustomerProductRates set AccountNo = '{accountNo}' where Id = {RateId};";
        //            var rs = await con.ExecuteAsync(query, transaction: trans);
        //            query = $@"select 
        //                        (select count(*) from CustomerProductRates where CustomerId = {CustomerId}) as Total,
        //                        (select count(*) from CustomerProductRates where CustomerId = {CustomerId} and AccountNo is not null) as Generated;";
        //            var customerAccountInfo = await con.QueryFirstOrDefaultAsync<(int Total, int Generated)>(query, transaction: trans);
        //            if (customerAccountInfo.Generated == customerAccountInfo.Total)
        //            {
        //                query = $@"update Customers set IsMatured = 1 where Id = {CustomerId};";
        //                rs = await con.ExecuteAsync(query, transaction: trans);
        //            }
        //            m = $"Account# {accountNo} Generated Successfully";
        //            s = true;
        //        }
        //        else
        //        {
        //            m = $"Account# {accountNo} Already Exists";
        //        }

        //        trans.Commit();
        //        con.Close();
        //        return (s, m);
        //    }
        //    catch (SqlException ex)
        //    {
        //        if (con.State == System.Data.ConnectionState.Open)
        //        {
        //            trans.Rollback();
        //            con.Close();
        //        }
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (con.State == System.Data.ConnectionState.Open)
        //        {
        //            trans.Rollback();
        //            con.Close();
        //        }
        //        throw ex;
        //    }
        //}
    }
}