using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MRaabta.Models;

namespace MRaabta.Repo
{
    public class RecoveryLetterRepo
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());

        public async Task<List<RecoveryLetterModel>> GetData(string AccountNo, string GroupId, string Days)
        {
            try
            {
                string query = $@"SELECT --{HttpContext.Current.Session["U_NAME"]}
                 * FROM (
                select  	
                cc.id 'ClientID',
                'IN-'+i.invoiceNumber 'Invoice',
                z.region 'Invoice Region',
                z.name 'Invoice Zone',
                b.name 'Invoice Branch',
                com.companyName 'Company',
                cc.accountNo 'Account Number',
                z.name+'-'+b.sname+'-'+cc.accountNo 'ClientAccount',
                cc.name 'ClientName',
                cc.phoneNo 'PhoneNo',
                isnull(cc.email,'') 'Email',
                --case when cc.id in ('345248','345249','345259','345260') then 'Aviation' when cc.id in ('345031','345281','345030') then 'M&P' else 'General' end 'Client Type',
                cc.address 'Client Address',
                cg.name  'Client Group',cc.clientGrpId 'Client Group ID',
                isnull(cg.description,'') 'ParantCode',
                case when t.Name is null then ''  else t.name end 'Industry',
                case when cc.centralizedClient='1' then 'Centralized' else ' 'end 'Centralized Indicator ',
                case when cc.IsCOD='1' then 'COD' else ' 'end 'COD Indicator ',
                case when cc.isActive='1' then 'Active' else 'In Active'end 'Client Status',
                case when (case when cc.centralizedClient='1' then zz.name else z.name end ) in ('KHI','HDD','SKZ','UET')then 'South'
                when (case when cc.centralizedClient='1' then zz.name else z.name end ) in ('FSD','LHE','MUX') then 'Central'
                when (case when cc.centralizedClient='1' then zz.name else z.name end ) in ('PEW','RWP','ISB','GUJ') then 'North' end 'Collection Center Region',
                case when cc.centralizedClient='1' then zz.name else z.name end 'Collection Center Zone',
                case when cc.centralizedClient='1' then bb.name else b.name end 'Collection Center Branch',
                case when bs1.firstName=bs1.lastName then bs1.firstName
                when bs1.lastName is null then bs1.firstName
                else bs1.firstName+' '+bs1.lastName end+' ('+bs1.usercode+')' as 'Sales Executive',
                case when bs2.firstName=bs2.lastName then bs2.firstName
                when bs2.lastName is null then bs2.firstName
                else bs2.firstName+' '+bs2.lastName end+' ('+bs2.usercode+')' as 'Recovery officer',
                --case when bs3.firstName=bs3.lastName then bs3.firstName
                --when bs3.lastName is null then bs3.firstName
                --else bs3.firstName+' '+bs3.lastName end+' ('+bs2.usercode+')' as 'Recovery officer2',
                datename(MM,i.startDate)+'-'+datename(YY,i.startDate) 'Invoice Month',
                case 	
                when MONTH(i.startDate)<'7' then cast((datename(YY,i.startDate)) as varchar)
                else cast(datename(YY,i.startDate)as varchar) end 'FiscalYear',
                case 	
                when (DATEDIFF(MONTH,i.startDate,getdate())) <'4' then '3 Month' 
                else 'More Then 3 months' end 'Months',
                CASE 	
                WHEN (DATEDIFF(MONTH, i.startDate, getdate())) <= '0' THEN '0-00 Days'
                WHEN (DATEDIFF(MONTH, i.startDate, getdate())) = '1' THEN  '0-30 Days'
                WHEN (DATEDIFF(MONTH, i.startDate, getdate())) = '2' THEN  '31-60 Days'
                WHEN (DATEDIFF(MONTH, i.startDate, getdate())) = '3' THEN  '61-90 Days'
                WHEN (DATEDIFF(MONTH, i.startDate, getdate())) = '4' THEN  '91-120 Days'
                WHEN (DATEDIFF(MONTH, i.startDate, getdate())) > '4' AND LEFT(i.startDate, 7)  >= '2016-01' THEN 'Above 120 till Jan-16'
                WHEN LEFT(i.startDate, 7) <= '2015-12' AND LEFT(i.startDate, 7) >=  '2015-07' THEN 'Jul-2015 to Dec-2015'
                ELSE  'Pre Acqusition'
                END TargetBucket, DATEDIFF(DAY,i.startDate,getdate()) TotalDays,
                case 	
                when left(i.startDate,7)  >='2015-07' then 'Current' 
                else 'Old' end 'Period',
                case when i.totalAmount is null then '0'  else i.totalAmount  end 'InvoiceAmount',
                round(case when i.totalAmount is null then '0'  else i.totalAmount  end -(case when rr1.RR is null then '0'  else rr1.RR  end +case when jv1.JV is null then '0'  else jv1.JV  end ),2) 'Outstanding as on 6th',
                round(case when i.totalAmount is null then '0'  else i.totalAmount  end -(case when rr2.RR is null then '0'  else rr2.RR  end +case when jv2.JV is null then '0'  else JV2.JV  end ),2) 'Outstanding',
                isnull(mmc.category,'') 'Recovery Status'
                from Invoice i 
                left join CreditClients cc on i.clientId=cc.id
                left join ClientGroups cg on cc.clientGrpId=cg.id
                left join Branches b on cc.branchCode=b.branchCode
                left join Zones z on b.zoneCode=z.zoneCode
                left join Branches bb on cg.collectionCenter=bb.branchCode
                left join Zones zz on bb.zoneCode=zz.zoneCode
                left join Company com on i.companyId=com.Id
                left join tblAdminIndustry t on cc.IndustryId=t.Id
                left join (select  cs1.* from (
                select cs.ClientId,cs.StaffTypeId,
                max(cs.id)'Max ID' from ClientStaff cs  
                where cs.StaffTypeId='214' 
                group by cs.ClientId,cs.StaffTypeId)tem 
                left JOIN ClientStaff cs1 on tem.[Max ID]=cs1.id )S on cc.id=s.ClientId
                left join (select  cs1.* from (
                select cs.ClientId,cs.StaffTypeId,
                max(cs.id)'Max ID' from ClientStaff cs  
                where cs.StaffTypeId='215' 
                group by cs.ClientId,cs.StaffTypeId)tem 
                left JOIN ClientStaff cs1 on tem.[Max ID]=cs1.id )S2 on cc.id=s2.ClientId
                --left join (select  cs1.* from (
                --select cs.ClientId,cs.StaffTypeId,
                --max(cs.id)'Max ID' from ClientStaff_temp cs  
                --where cs.StaffTypeId='215' 
                --group by cs.ClientId,cs.StaffTypeId)tem 
                --left JOIN ClientStaff_temp cs1 on tem.[Max ID]=cs1.id )S3 on cc.id=s3.ClientId
                left join BTSUsers bs1 on S.UserName=bs1.username
                left join BTSUsers bs2 on S2.UserName=bs2.username
                --left join BTSUsers BS3 on S3.comp3=BS3.usercode
                left join mnp_recoveryvisit rv
                            on rv.invoicenumber = i.invoicenumber
                           and rv.created_On =
                               (select MAX(rv1.created_On)
                                  from mnp_recoveryvisit rv1
                                 where rv1.invoicenumber = rv.invoicenumber)
                left join mnp_major_category mmc on rv.major_category = mmc.id
                left join (	
                select  i1.invoiceNumber, sum(g.Amount)JV from Invoice i1  
                inner join GeneralVoucher g on i1.invoiceNumber=g.InvoiceNo 
                where g.VoucherDate < CAST(GETDATE() AS DATE)	 group by i1.invoiceNumber) JV1 on i.invoiceNumber=jv1.invoiceNumber	
                left join (	
                select i2.invoiceNumber, sum(ir.Amount)RR from Invoice i2  
                inner join InvoiceRedeem ir on i2.invoiceNumber=ir.InvoiceNo 
                left join PaymentVouchers a on ir.PaymentVoucherId=a.Id
                left join PaymentSource b on a.PaymentSourceId=b.Id --AND b.id NOT IN ('5')
                left join PaymentTypes f on a.PaymentTypeId=f.Id
                left join ChequeStatus cs1 on a.Id=cs1.PaymentVoucherId
                left join ChequeState cs2 on cs1.ChequeStateId=cs2.Id and cs1.IsCurrentState = '1'
                where a.VoucherDate < CAST(GETDATE() AS DATE)	 	
                --and case WHEN isnull(b.Name,'CASH') = 'Credit' THEN cs2.Name ELSE 'CASH' end not in ('Bounced','Returned') --NEW
                and case WHEN ISNULL(b.Name, 'Cash') IN ('CasH','Demand Draft','Pay Order','Tax Challan','QR Code','Credit Card','Adjustment') 
                then 'Cleared' else isnull(cs2.Name,'') end not in ('Bounced','Returned')  -- OLD
                group by i2.invoiceNumber) RR1 on i.invoiceNumber=rr1.invoiceNumber
                left join (	
                select  i1.invoiceNumber, sum(g.Amount)JV from Invoice i1  
                inner join GeneralVoucher g on i1.invoiceNumber=g.InvoiceNo 
                where g.VoucherDate <= CAST(GETDATE() AS DATE)	 group by i1.invoiceNumber) JV2 on i.invoiceNumber=jv2.invoiceNumber	
                left join (	
                select i2.invoiceNumber, sum(ir.Amount)RR from Invoice i2  
                inner join InvoiceRedeem ir on i2.invoiceNumber=ir.InvoiceNo 
                left join PaymentVouchers a on ir.PaymentVoucherId=a.Id
                left join PaymentSource b on a.PaymentSourceId=b.Id --AND b.id NOT IN ('5')
                left join PaymentTypes f on a.PaymentTypeId=f.Id
                left join ChequeStatus cs1 on a.Id=cs1.PaymentVoucherId
                left join ChequeState cs2 on cs1.ChequeStateId=cs2.Id and cs1.IsCurrentState = '1'
                where a.VoucherDate <= CAST(GETDATE() AS DATE)	
                --and case WHEN isnull(b.Name,'CASH') = 'Credit' THEN cs2.Name ELSE 'CASH' end not in ('Bounced','Returned') --NEW
                and case WHEN ISNULL(b.Name, 'Cash') IN ('CasH','Demand Draft','Pay Order','Tax Challan','QR Code','Credit Card','Adjustment') 
                then 'Cleared' else isnull(cs2.Name,'') end not in ('Bounced','Returned')  -- OLD
                group by i2.invoiceNumber) RR2 on i.invoiceNumber=rr2.invoiceNumber
                where 	
                (i.IsInvoiceCanceled IS NULL OR i.IsInvoiceCanceled ='0')
                and LEFT(i.startDate, 7) >= '2015-07'
                and round(case when i.totalAmount is null then '0'  else i.totalAmount  end -(case when rr1.RR is null then '0'  else rr1.RR  end +case when jv1.JV is null then '0'  else jv1.JV  end ),2) != '0'
                --AND i.invoiceNumber = '1202010013127'
                ) x WHERE x.TargetBucket = '{Days}' AND x.[InvoiceAmount]>=500 and x.Outstanding not between -100 and 100 ";

                if (GroupId != "")
                {
                    query += $@" AND x.[Client Group ID]= '{GroupId}' ";
                }
                if (AccountNo != "")
                {
                    query += $@" AND x.[Account Number]='{AccountNo}'";
                }

                var rs = await con.QueryAsync<RecoveryLetterModel>(query);
                con.Close();
                List<RecoveryLetterModel> s = new List<RecoveryLetterModel>();
                s = (List<RecoveryLetterModel>)rs;
                return s.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }

        public async Task<List<RecoveryLetterDetailModel>> GetDetailData(string InvoiceNumber, string Days)
        {
            try
            {
                string condition = "";
                if (InvoiceNumber.Contains("IN"))
                {
                    condition = $@" and x.[Invoice] IN ({InvoiceNumber})";
                }
                else
                {
                    condition = $@" and x.[ClientID] IN ({InvoiceNumber})";
                }
                string query = $@"SELECT --{HttpContext.Current.Session["U_NAME"]}
                     * FROM (
                    select  	
                    cc.id 'ClientID',
                    'IN-'+i.invoiceNumber 'Invoice',
                    z.region 'Invoice Region',
                    z.name 'InvoiceZone',
                    b.name 'InvoiceBranch',
                    com.companyName 'Company',
                    cc.accountNo 'AccountNumber',
                    z.name+'-'+b.sname+'-'+cc.accountNo 'ClientAccount',
                    cc.name 'ClientName',
                    cc.phoneNo 'PhoneNo',
                    isnull(cc.email,'') 'Email',
                    --case when cc.id in ('345248','345249','345259','345260') then 'Aviation' when cc.id in ('345031','345281','345030') then 'M&P' else 'General' end 'Client Type',
                    cc.address 'ClientAddress',
                    cg.name  'Client Group',cc.clientGrpId 'Client Group ID',
                    isnull(cg.description,'') 'ParantCode',
                    case when t.Name is null then ''  else t.name end 'Industry',
                    case when cc.centralizedClient='1' then 'Centralized' else ' 'end 'Centralized Indicator ',
                    case when cc.IsCOD='1' then 'COD' else ' 'end 'COD Indicator ',
                    case when cc.isActive='1' then 'Active' else 'In Active'end 'Client Status',
                    case when (case when cc.centralizedClient='1' then zz.name else z.name end ) in ('KHI','HDD','SKZ','UET')then 'South'
                    when (case when cc.centralizedClient='1' then zz.name else z.name end ) in ('FSD','LHE','MUX') then 'Central'
                    when (case when cc.centralizedClient='1' then zz.name else z.name end ) in ('PEW','RWP','ISB','GUJ') then 'North' end 'Collection Center Region',
                    case when cc.centralizedClient='1' then zz.name else z.name end 'Collection Center Zone',
                    case when cc.centralizedClient='1' then bb.name else b.name end 'Collection Center Branch',
                    case when bs1.firstName=bs1.lastName then bs1.firstName
                    when bs1.lastName is null then bs1.firstName
                    else bs1.firstName+' '+bs1.lastName end+' ('+bs1.usercode+')' as 'Sales Executive',
                    case when bs2.firstName=bs2.lastName then bs2.firstName
                    when bs2.lastName is null then bs2.firstName
                    else bs2.firstName+' '+bs2.lastName end+' ('+bs2.usercode+')' as 'Recovery officer',
                    --case when bs3.firstName=bs3.lastName then bs3.firstName
                    --when bs3.lastName is null then bs3.firstName
                    --else bs3.firstName+' '+bs3.lastName end+' ('+bs2.usercode+')' as 'Recovery officer2',
                    datename(MM,i.startDate)+'-'+datename(YY,i.startDate) 'InvoiceMonth',
                    case 	
                    when MONTH(i.startDate)<'7' then cast((datename(YY,i.startDate)) as varchar)
                    else cast(datename(YY,i.startDate)as varchar) end 'FiscalYear',
                    case 	
                    when (DATEDIFF(MONTH,i.startDate,getdate())) <'4' then '3 Month' 
                    else 'More Then 3 months' end 'Months',
                    CASE 	
                    WHEN (DATEDIFF(MONTH, i.startDate, getdate())) <= '0' THEN '0-00 Days'
                    WHEN (DATEDIFF(MONTH, i.startDate, getdate())) = '1' THEN  '0-30 Days'
                    WHEN (DATEDIFF(MONTH, i.startDate, getdate())) = '2' THEN  '31-60 Days'
                    WHEN (DATEDIFF(MONTH, i.startDate, getdate())) = '3' THEN  '61-90 Days'
                    WHEN (DATEDIFF(MONTH, i.startDate, getdate())) = '4' THEN  '91-120 Days'
                    WHEN (DATEDIFF(MONTH, i.startDate, getdate())) > '4' AND LEFT(i.startDate, 7)  >= '2016-01' THEN 'Above 120 till Jan-16'
                    WHEN LEFT(i.startDate, 7) <= '2015-12' AND LEFT(i.startDate, 7) >=  '2015-07' THEN 'Jul-2015 to Dec-2015'
                    ELSE  'Pre Acqusition'
                    END TargetBucket, DATEDIFF(DAY,i.startDate,getdate()) TotalDays,
                    case 	
                    when left(i.startDate,7)  >='2015-07' then 'Current' 
                    else 'Old' end 'Period',
                    case when i.totalAmount is null then '0'  else i.totalAmount  end 'InvoiceAmount',
                    round(case when i.totalAmount is null then '0'  else i.totalAmount  end -(case when rr1.RR is null then '0'  else rr1.RR  end +case when jv1.JV is null then '0'  else jv1.JV  end ),2) 'Outstanding as on 6th',
                    round(case when i.totalAmount is null then '0'  else i.totalAmount  end -(case when rr2.RR is null then '0'  else rr2.RR  end +case when jv2.JV is null then '0'  else JV2.JV  end ),2) 'Outstanding',
                    isnull(mmc.category,'') 'Recovery Status'
                    from Invoice i 
                    left join CreditClients cc on i.clientId=cc.id
                    left join ClientGroups cg on cc.clientGrpId=cg.id
                    left join Branches b on cc.branchCode=b.branchCode
                    left join Zones z on b.zoneCode=z.zoneCode
                    left join Branches bb on cg.collectionCenter=bb.branchCode
                    left join Zones zz on bb.zoneCode=zz.zoneCode
                    left join Company com on i.companyId=com.Id
                    left join tblAdminIndustry t on cc.IndustryId=t.Id
                    left join (select  cs1.* from (
                    select cs.ClientId,cs.StaffTypeId,
                    max(cs.id)'Max ID' from ClientStaff cs  
                    where cs.StaffTypeId='214' 
                    group by cs.ClientId,cs.StaffTypeId)tem 
                    left JOIN ClientStaff cs1 on tem.[Max ID]=cs1.id )S on cc.id=s.ClientId
                    left join (select  cs1.* from (
                    select cs.ClientId,cs.StaffTypeId,
                    max(cs.id)'Max ID' from ClientStaff cs  
                    where cs.StaffTypeId='215' 
                    group by cs.ClientId,cs.StaffTypeId)tem 
                    left JOIN ClientStaff cs1 on tem.[Max ID]=cs1.id )S2 on cc.id=s2.ClientId
                    --left join (select  cs1.* from (
                    --select cs.ClientId,cs.StaffTypeId,
                    --max(cs.id)'Max ID' from ClientStaff_temp cs  
                    --where cs.StaffTypeId='215' 
                    --group by cs.ClientId,cs.StaffTypeId)tem 
                    --left JOIN ClientStaff_temp cs1 on tem.[Max ID]=cs1.id )S3 on cc.id=s3.ClientId
                    left join BTSUsers bs1 on S.UserName=bs1.username
                    left join BTSUsers bs2 on S2.UserName=bs2.username
                    --left join BTSUsers BS3 on S3.comp3=BS3.usercode
                    left join mnp_recoveryvisit rv
                                on rv.invoicenumber = i.invoicenumber
                               and rv.created_On =
                                   (select MAX(rv1.created_On)
                                      from mnp_recoveryvisit rv1
                                     where rv1.invoicenumber = rv.invoicenumber)
                    left join mnp_major_category mmc on rv.major_category = mmc.id
                    left join (	
                    select  i1.invoiceNumber, sum(g.Amount)JV from Invoice i1  
                    inner join GeneralVoucher g on i1.invoiceNumber=g.InvoiceNo 
                    where g.VoucherDate < CAST(GETDATE() AS DATE)	 group by i1.invoiceNumber) JV1 on i.invoiceNumber=jv1.invoiceNumber	
                    left join (	
                    select i2.invoiceNumber, sum(ir.Amount)RR from Invoice i2  
                    inner join InvoiceRedeem ir on i2.invoiceNumber=ir.InvoiceNo 
                    left join PaymentVouchers a on ir.PaymentVoucherId=a.Id
                    left join PaymentSource b on a.PaymentSourceId=b.Id --AND b.id NOT IN ('5')
                    left join PaymentTypes f on a.PaymentTypeId=f.Id
                    left join ChequeStatus cs1 on a.Id=cs1.PaymentVoucherId
                    left join ChequeState cs2 on cs1.ChequeStateId=cs2.Id and cs1.IsCurrentState = '1'
                    where a.VoucherDate < CAST(GETDATE() AS DATE)	 	
                    --and case WHEN isnull(b.Name,'CASH') = 'Credit' THEN cs2.Name ELSE 'CASH' end not in ('Bounced','Returned') --NEW
                    and case WHEN ISNULL(b.Name, 'Cash') IN ('CasH','Demand Draft','Pay Order','Tax Challan','QR Code','Credit Card','Adjustment') 
                    then 'Cleared' else isnull(cs2.Name,'') end not in ('Bounced','Returned')  -- OLD
                    group by i2.invoiceNumber) RR1 on i.invoiceNumber=rr1.invoiceNumber
                    left join (	
                    select  i1.invoiceNumber, sum(g.Amount)JV from Invoice i1  
                    inner join GeneralVoucher g on i1.invoiceNumber=g.InvoiceNo 
                    where g.VoucherDate <= CAST(GETDATE() AS DATE)	 group by i1.invoiceNumber) JV2 on i.invoiceNumber=jv2.invoiceNumber	
                    left join (	
                    select i2.invoiceNumber, sum(ir.Amount)RR from Invoice i2  
                    inner join InvoiceRedeem ir on i2.invoiceNumber=ir.InvoiceNo 
                    left join PaymentVouchers a on ir.PaymentVoucherId=a.Id
                    left join PaymentSource b on a.PaymentSourceId=b.Id --AND b.id NOT IN ('5')
                    left join PaymentTypes f on a.PaymentTypeId=f.Id
                    left join ChequeStatus cs1 on a.Id=cs1.PaymentVoucherId
                    left join ChequeState cs2 on cs1.ChequeStateId=cs2.Id and cs1.IsCurrentState = '1'
                    where a.VoucherDate <= CAST(GETDATE() AS DATE)	
                    --and case WHEN isnull(b.Name,'CASH') = 'Credit' THEN cs2.Name ELSE 'CASH' end not in ('Bounced','Returned') --NEW
                    and case WHEN ISNULL(b.Name, 'Cash') IN ('CasH','Demand Draft','Pay Order','Tax Challan','QR Code','Credit Card','Adjustment') 
                    then 'Cleared' else isnull(cs2.Name,'') end not in ('Bounced','Returned')  -- OLD
                    group by i2.invoiceNumber) RR2 on i.invoiceNumber=rr2.invoiceNumber
                    where 	
                    (i.IsInvoiceCanceled IS NULL OR i.IsInvoiceCanceled ='0')
                    and LEFT(i.startDate, 7) >= '2015-07'
                    and round(case when i.totalAmount is null then '0'  else i.totalAmount  end -(case when rr1.RR is null then '0'  else rr1.RR  end +case when jv1.JV is null then '0'  else jv1.JV  end ),2) != '0'
                    --AND i.invoiceNumber = '1202010013127'
                    ) x WHERE x.TargetBucket = '{Days}' AND x.[InvoiceAmount]>=500 and x.Outstanding not between -100 and 100 " + condition;

                await con.OpenAsync();
                var rs = await con.QueryAsync<RecoveryLetterDetailModel>(query, commandTimeout: int.MaxValue);
                con.Close();
                return rs.ToList();
            }
            catch (SqlException ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
    }
}