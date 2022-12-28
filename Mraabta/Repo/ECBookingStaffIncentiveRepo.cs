using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MRaabta.Repo
{
    public class ECBookingStaffIncentiveRepo
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
        // SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString_LIVE_SERVER_new"].ToString());

        public async Task<List<ECBookingStaffIncentiveReportModel>> GetReport(string Month, string Year)
        {
            try
            {
                string RiderCode = HttpContext.Current.Session["BOOKINGstaff"].ToString();
                string ExpressCenter = HttpContext.Current.Session["ExpressCenter"].ToString();

                string query = $@"select
                ----- retail incentive report
                 --- { HttpContext.Current.Session["U_NAME"]}
                cast(accountreceivingdate as date) as accountreceivingdate,zone,branch, ---ecname,originexpresscenter, ecname+' ('+originexpresscenter+')' ec_name_code,  
ridername, ridercode,ridername+' ('+ridercode+')' rider_code,   
separationtype,dateofleaving,usertypeid,  
sum(isnull(cncount,'0')) cncount,sum(isnull(domestic,'0')) domestic,sum(isnull([my air cargo],'0')) myaircargo,   
sum(isnull(international,'0')) international, sum(isnull([fedex],'0')) fedex, sum(isnull([road n rail],'0')) roadnrail, 
sum(isnull(domestic,'0'))+sum(isnull([my air cargo],'0'))+sum(isnull(international,'0'))+sum(isnull([fedex],'0'))+sum(isnull([road n rail],'0')) totalincentive   
from (   

select   
y.accountreceivingdate,y.zone,y.branch,y.ecname,y.originexpresscenter,y.ridername, y.ridercode,separationtype, dateofleaving,   
y.usertypeid, product,   
sum(y.cncount) cncount,   
isnull(sum(y.different),0) + isnull(sum(y.same),0) + isnull(sum(y.local),0) total_incentive   

from (  
    (   

select   
    x1.accountreceivingdate,x1.zone, x1.branch,   
    x1.ecname,x1.originexpresscenter,   
    x1.ridername,x1.ridercode,separationtype, dateofleaving,   
    x1.usertypeid,   
    'domestic' product,   
    sum(x1.cncount) cncount,   
    case when x1.zoning = 'different' then SUM(x1.totalincentive) else 0 end different,   
    case when x1.zoning = 'same' then SUM(x1.totalincentive) else 0 end same,   
    case when x1.zoning = 'local' then SUM(x1.totalincentive) else 0 end local   
    from (   

-- domestic product   
select   
x.accountreceivingdate,x.zone, x.branch,x.branchcode, x.ecname, x.originexpresscenter, x.ridercode, x.ridername,separationtype, dateofleaving, x.usertypeid, x.zoning, x.weight,   
sum(x.cncount) cncount, 
CASE 
	WHEN X.serviceTypeName = 'Mango Fiesta 5 Kg' THEN '30' 
	WHEN X.serviceTypeName = 'Mango Fiesta 7 kg' THEN '35' 
	WHEN X.serviceTypeName = 'Mango Fiesta 10 kg' THEN '40' 
	--WHEN X.ModifierName IN ('Cellophane Wrap Small Boxes','Cellophane Wrap Large Boxes') AND X.[WEIGHT] <= '10' THEN '7' 
	--WHEN X.ModifierName IN ('Cellophane Wrap Small Boxes','Cellophane Wrap Large Boxes') AND X.[WEIGHT] > '10' THEN '10'	
	ELSE INCENTIVERATE END INCENTIVERATE,  
(sum(X.cncount) * 
	CASE 
	WHEN X.serviceTypeName = 'Mango Fiesta 5 Kg' THEN '30' 
	WHEN X.serviceTypeName = 'Mango Fiesta 7 kg' THEN '35' 
	WHEN X.serviceTypeName = 'Mango Fiesta 10 kg' THEN '40' 
	--WHEN X.ModifierName IN ('Cellophane Wrap Small Boxes','Cellophane Wrap Large Boxes') AND X.[WEIGHT] <= '10' THEN '7' * SUM(x.pieces)
	--WHEN X.ModifierName IN ('Cellophane Wrap Small Boxes','Cellophane Wrap Large Boxes') AND X.[WEIGHT] > '10' THEN '10' * SUM(x.pieces)
	ELSE INCENTIVERATE END
	) TOTALINCENTIVE    
from (   
select   
b.accountreceivingdate,b.zone, b.branch,b.branchcode, b.originexpresscenter, b.ridercode, b.ridername, separationtype, b.dateofleaving,b.usertypeid, b.zoning, ec.name ecname,  
--ModifierName,pieces,
B.serviceTypeName, 
    case when b.weight > 0 and b.weight <= 0.5 then 0.5   
        when b.weight > 0.5 and b.weight <= 1.0 then 1   
        when b.weight > 1.0 and b.weight <= 1.5 then 1.5   
        when b.weight > 1.5 and b.weight <= 2.0 then 2   
        when b.weight > 2.0 and b.weight <= 3.0 then 3   
        when b.weight > 3.0 and b.weight <= 4.0 then 4   
        when b.weight > 4.0 and b.weight <= 5.0 then 5   
        when b.weight > 5.0 and b.weight <= 6.0 then 6   
        when b.weight > 6.0 and b.weight <= 7.0 then 7   
        when b.weight > 7.0 and b.weight <= 8.0 then 8   
        when b.weight > 8.0 and b.weight <= 9.0 then 9   
        when b.weight > 9.0 and b.weight <= 10.0 then 10   
        when b.weight > 10.0 and b.weight <= 11.0 then 11   
        when b.weight > 11.0 and b.weight <= 12.0 then 12   
        when b.weight > 12.0 and b.weight <= 13.0 then 13   
        when b.weight > 13.0 and b.weight <= 14.0 then 14   
        when b.weight > 14.0 and b.weight <= 15.0 then 15   
        when b.weight > 15.0 and b.weight <= 16.0 then 16   
        when b.weight > 16.0 and b.weight <= 17.0 then 17   
        when b.weight > 17.0 and b.weight <= 18.0 then 18   
        when b.weight > 18.0 and b.weight <= 19.0 then 19   
        when b.weight > 19.0 and b.weight <= 20.0 then 20   
        when b.weight > 20.0 and b.weight <= 21.0 then 21   
        when b.weight > 21.0 and b.weight <= 22.0 then 22   
        when b.weight > 22.0 and b.weight <= 23.0 then 23   
        when b.weight > 23.0 and b.weight <= 24.0 then 24   
        when b.weight > 24.0 and b.weight <= 25.0 then 25   
        when b.weight > 25.0 and b.weight <= 26.0 then 26   
        when b.weight > 26.0 and b.weight <= 27.0 then 27   
        when b.weight > 27.0 and b.weight <= 28.0 then 28   
        when b.weight > 28.0 and b.weight <= 29.0 then 29   
        when b.weight > 29.0 and b.weight <= 30.0 then 30   
        when b.weight > 30.0 and b.weight <= 31.0 then 31   
        when b.weight > 31.0 and b.weight <= 32.0 then 32   
        when b.weight > 32.0 and b.weight <= 33.0 then 33   
        when b.weight > 33.0 and b.weight <= 34.0 then 34   
        when b.weight > 34.0 and b.weight <= 35.0 then 35   
        when b.weight > 35.0 and b.weight <= 36.0 then 36   
        when b.weight > 36.0 and b.weight <= 37.0 then 37   
        when b.weight > 37.0 and b.weight <= 38.0 then 38   
        when b.weight > 38.0 and b.weight <= 39.0 then 39   
        when b.weight > 39.0 and b.weight <= 40.0 then 40   
        when b.weight > 40.0 and b.weight <= 41.0 then 41   
        when b.weight > 41.0 and b.weight <= 42.0 then 42   
        when b.weight > 42.0 and b.weight <= 43.0 then 43   
        when b.weight > 43.0 and b.weight <= 44.0 then 44   
        when b.weight > 44.0 and b.weight <= 45.0 then 45   
        when b.weight > 45.0 and b.weight <= 46.0 then 46   
        when b.weight > 46.0 and b.weight <= 47.0 then 47   
        when b.weight > 47.0 and b.weight <= 48.0 then 48   
        when b.weight > 48.0 and b.weight <= 49.0 then 49   
        when b.weight > 49.0 then 50   
    end weight,   
    sum(b.cn) cncount   
from (   
select   
    c.accountreceivingdate, z.name zone,   
    b.name branch,b.branchcode,c.originexpresscenter,   
    r.ridercode ridercode,   
    case when r.lastname is null then r.firstname   
    when r.firstname = r.lastname then r.firstname   
    else r.firstname+' '+r.lastname end ridername,   
    case when l.attributevalue is null then r.usertypeid else l.attributevalue end usertypeid,   
    case   
        when c.orgin = c.destination then 'local'   
        when z.colorid = zz.colorid then 'same'   
        else 'different'   
    end zoning,   
    isnull(c.weight,0) weight,   
    count(c.consignmentnumber) cn, r.separationtype, r.dateofleaving,   
    C.serviceTypeName
	--,PM.name ModifierName, c.pieces  
    from   
    consignment c   
    inner join branches b on c.orgin = b.branchcode   
    inner join zones z on b.zonecode = z.zonecode   
    inner join branches bb on c.destination=bb.branchcode   
    inner join zones zz on bb.zonecode = zz.zonecode   
    inner join riders r on c.ridercode = r.ridercode and c.orgin = r.branchid   
    inner join servicetypes_new st on st.servicetypename = c.servicetypename   
    left join rvdbo.lookup l on r.usertypeid = cast(l.id as varchar)   
    --LEFT JOIN ConsignmentModifier cm ON cm.consignmentNumber = c.consignmentNumber
    --LEFT JOIN PriceModifiers AS pm ON pm.id = cm.priceModifierId AND pm.id IN ('111','112')
    where  
		month(c.accountreceivingdate)= '{Month}' and YEAR(c.accountreceivingdate)= '{Year}'
		and st.products = 'domestic' and st.servicetypename != 'my air cargo'   
		and isnull(c.isapproved,'0') = '1'    
		and isnull(c.ispricecomputed,'0') = '1'
		AND (C.CONSIGNERACCOUNTNO = '0' OR C.consignerAccountNo LIKE '%CC%')
		and r.status = '1'
		AND c.expressCenterCode='{ExpressCenter}'
        AND c.consignmentNumber NOT IN (
             SELECT mpbc.consignmentNumber
               FROM MNP_PreBookingConsignment AS mpbc WHERE mpbc.consignmentNumber = c.consignmentNumber    
             ) 

    group by   
    z.name,   c.accountreceivingdate,
    b.name,b.branchcode,c.originexpresscenter,   
    r.ridercode, c.weight,      
    case when r.lastname is null then r.firstname   
    when r.firstname = r.lastname then r.firstname   
    else r.firstname+' '+r.lastname end,   
    case when l.attributevalue is null then r.usertypeid else l.attributevalue end,   
    case when c.orgin = c.destination then 'local'   
    when z.colorid = zz.colorid then 'same' else 'different' end, r.separationtype, r.dateofleaving,
    C.serviceTypeName
	--,PM.name, c.pieces   
    ) b   
left join expresscenters ec on b.originexpresscenter = ec.expresscentercode   
where    
--b.zoning = 'different'   
ec.center_type = '1'  and b.ridercode = '{RiderCode}'	    
group by   
b.zone, b.branch,b.branchcode, ec.name,b.originexpresscenter, b.ridercode, b.ridername, separationtype, b.dateofleaving,b.usertypeid, b.zoning,  b.accountreceivingdate, 
--ModifierName,pieces,
B.serviceTypeName,
    case when b.weight > 0 and b.weight <= 0.5 then 0.5   
        when b.weight > 0.5 and b.weight <= 1.0 then 1   
        when b.weight > 1.0 and b.weight <= 1.5 then 1.5   
        when b.weight > 1.5 and b.weight <= 2.0 then 2   
        when b.weight > 2.0 and b.weight <= 3.0 then 3   
        when b.weight > 3.0 and b.weight <= 4.0 then 4   
        when b.weight > 4.0 and b.weight <= 5.0 then 5   
        when b.weight > 5.0 and b.weight <= 6.0 then 6   
        when b.weight > 6.0 and b.weight <= 7.0 then 7   
        when b.weight > 7.0 and b.weight <= 8.0 then 8   
        when b.weight > 8.0 and b.weight <= 9.0 then 9   
        when b.weight > 9.0 and b.weight <= 10.0 then 10   
        when b.weight > 10.0 and b.weight <= 11.0 then 11   
        when b.weight > 11.0 and b.weight <= 12.0 then 12   
        when b.weight > 12.0 and b.weight <= 13.0 then 13   
        when b.weight > 13.0 and b.weight <= 14.0 then 14   
        when b.weight > 14.0 and b.weight <= 15.0 then 15   
        when b.weight > 15.0 and b.weight <= 16.0 then 16   
        when b.weight > 16.0 and b.weight <= 17.0 then 17   
        when b.weight > 17.0 and b.weight <= 18.0 then 18   
        when b.weight > 18.0 and b.weight <= 19.0 then 19   
        when b.weight > 19.0 and b.weight <= 20.0 then 20   
        when b.weight > 20.0 and b.weight <= 21.0 then 21   
        when b.weight > 21.0 and b.weight <= 22.0 then 22   
        when b.weight > 22.0 and b.weight <= 23.0 then 23   
        when b.weight > 23.0 and b.weight <= 24.0 then 24   
        when b.weight > 24.0 and b.weight <= 25.0 then 25   
        when b.weight > 25.0 and b.weight <= 26.0 then 26   
        when b.weight > 26.0 and b.weight <= 27.0 then 27   
        when b.weight > 27.0 and b.weight <= 28.0 then 28   
        when b.weight > 28.0 and b.weight <= 29.0 then 29   
        when b.weight > 29.0 and b.weight <= 30.0 then 30   
        when b.weight > 30.0 and b.weight <= 31.0 then 31   
        when b.weight > 31.0 and b.weight <= 32.0 then 32   
        when b.weight > 32.0 and b.weight <= 33.0 then 33   
        when b.weight > 33.0 and b.weight <= 34.0 then 34   
        when b.weight > 34.0 and b.weight <= 35.0 then 35   
        when b.weight > 35.0 and b.weight <= 36.0 then 36   
        when b.weight > 36.0 and b.weight <= 37.0 then 37   
        when b.weight > 37.0 and b.weight <= 38.0 then 38   
        when b.weight > 38.0 and b.weight <= 39.0 then 39   
        when b.weight > 39.0 and b.weight <= 40.0 then 40   
        when b.weight > 40.0 and b.weight <= 41.0 then 41   
        when b.weight > 41.0 and b.weight <= 42.0 then 42   
        when b.weight > 42.0 and b.weight <= 43.0 then 43   
        when b.weight > 43.0 and b.weight <= 44.0 then 44   
        when b.weight > 44.0 and b.weight <= 45.0 then 45   
        when b.weight > 45.0 and b.weight <= 46.0 then 46   
        when b.weight > 46.0 and b.weight <= 47.0 then 47   
        when b.weight > 47.0 and b.weight <= 48.0 then 48   
        when b.weight > 48.0 and b.weight <= 49.0 then 49   
        when b.weight > 49.0 then 50   
    end   
    ) x   
    inner join mnp_riderincentiverates rir on x.weight = rir.weight and x.zoning = rir.incentivetype 
   -- LEFT JOIN ConsignmentModifier cm ON cm.consignmentNumber = c.consignmentNumber
 --   LEFT JOIN PriceModifiers AS pm ON pm.id = cm.priceModifierId AND pm.id IN ('111','112')  
    group by   
    x.accountreceivingdate,x.zone, x.branch,x.branchcode, x.ecname,x.originexpresscenter, x.ridercode, x.ridername, separationtype,   
    dateofleaving,x.usertypeid, x.zoning, x.weight,  incentiverate   ,X.serviceTypeName--,X.ModifierName 
) x1   
group by   
x1.zone, x1.branch,   x1.accountreceivingdate,
x1.ecname,x1.originexpresscenter,   
x1.ridername, x1.ridercode,separationtype, dateofleaving,   
x1.usertypeid,x1.zoning,   
x1.totalincentive,x1.incentiverate   
)   


union all   

----------------------- my air cargo -----------------------   
(   
select   
y.accountreceivingdate,y.zone,y.branch,y.ecname,y.expresscentercode originexpresscenter,y.ridername,y.ridercode,separationtype, dateofleaving,   
y.usertypeid, product,   
sum(y.cncount) cncount,   
isnull(sum(y.different),0) different,   
isnull(sum(y.same),0) same,   
isnull(sum(y.local),0) local   
from (   
    select   
    x.accountreceivingdate,x.zone,x.branch,x.ridername,x.ridercode, separationtype, dateofleaving,--x.ecname ,       
    x.ecname,x.expresscentercode ,   
    x.usertypeid,   
    'my air cargo' product,   
    sum(x.cncount) cncount,   
    case when x.zoning = 'different' then sum(x.incentiverate) end different,   
    case when x.zoning = 'same' then sum(x.incentiverate) end same,   
    case when x.zoning = 'local' then sum(x.incentiverate) end local   
    from (   

-- different   
select   
    b.accountreceivingdate,b.zone,b.branch,b.ridercode,b.ridername, separationtype, b.dateofleaving,b.usertypeid, b.zoning, ec.name ecname, ec.expresscentercode,   
    case   
    when b.weight > 0 and b.weight < 50 then 50   
    else b.weight   
    end weight,   
    sum(b.cn) cncount,   
    b.incentiverate incentiverateperweight,   
    (case   
    when b.zoning = 'local' and b.weight >= 0 and b.weight <= 50 then (50 * 1)  
    when b.zoning = 'same' and b.weight >= 0 and b.weight <= 50 then (50 * 1.5)  
    when b.zoning = 'different' and b.weight >= 0 and b.weight <= 50 then (50 * 2)   
    when b.zoning = 'local' and b.weight > 50 then (b.weight * 1)  
    when b.zoning = 'same' and b.weight > 50 then (b.weight * 1.5)  
    when b.zoning = 'different' and b.weight > 50 then (b.weight * 2)  
    end) incentiverate   
from   
(   
    select   
    c.accountreceivingdate, z.name zone,   
    b.name branch,   
    b.branchcode,   
    r.ridercode ridercode,   
    case when r.lastname is null then r.firstname   
    when r.firstname = r.lastname then r.firstname   
    else r.firstname+' '+r.lastname end ridername,separationtype, dateofleaving,   
    case when l.attributevalue is null then r.usertypeid else l.attributevalue end usertypeid,   
    case   
        when c.orgin = c.destination then 'local'   
        when z.colorid = zz.colorid then 'same'   
        else 'different'   
    end zoning,   
    isnull(c.weight,0) weight,   
    -- ec.name,   
    count(c.consignmentnumber) cn,   
    '5' incentiverate,   
    c.originexpresscenter   
    from   
    consignment c   
    inner join branches b on c.orgin = b.branchcode   
    inner join zones z on b.zonecode = z.zonecode   
    inner join branches bb on c.destination=bb.branchcode   
    inner join zones zz on bb.zonecode = zz.zonecode   
    inner join riders r on c.ridercode = r.ridercode and c.orgin = r.branchid   
    --inner join mnp_riderincentiverates rir on c.weight = rir.weight   
    inner join servicetypes_new st on st.servicetypename = c.servicetypename   
    left join rvdbo.lookup l on r.usertypeid = cast(l.id as varchar)   
    --inner join expresscenters ec on bb.branchcode = ec.bid   
    where  
    month(c.accountreceivingdate)= '{Month}' and YEAR(c.accountreceivingdate)= '{Year}'
    and st.products = 'domestic' and st.servicetypename = 'my air cargo'   
    and isnull(c.isapproved,'0') = '1'   
    and isnull(c.ispricecomputed,'0') = '1' and r.status = '1'  AND c.expressCenterCode='{ExpressCenter}' 
    and c.consigneraccountno = '0'   
    AND c.consignmentNumber NOT IN (
             SELECT mpbc.consignmentNumber
               FROM MNP_PreBookingConsignment AS mpbc WHERE mpbc.consignmentNumber = c.consignmentNumber    
             ) 

    group by   
    c.accountreceivingdate,z.name,   
    b.name,b.branchcode,c.originexpresscenter,separationtype, dateofleaving,   
    r.ridercode, c.weight, --ec.name,   
    --rir.incentiverate,   
    case when r.lastname is null then r.firstname   
    when r.firstname = r.lastname then r.firstname   
    else r.firstname+' '+r.lastname end,   
    case when l.attributevalue is null then r.usertypeid else l.attributevalue end,   
    case when c.orgin = c.destination then 'local'   
    when z.colorid = zz.colorid then 'same' else 'different' end   
) b   
left join expresscenters ec on b.originexpresscenter = ec.expresscentercode   
where    
ec.center_type = '1'  and b.ridercode = '{RiderCode}'	    
group by   
    b.accountreceivingdate, b.zone,b.branch,b.ridercode,b.ridername, separationtype, b.dateofleaving,b.usertypeid, b.zoning, b.incentiverate,ec.name,ec.expresscentercode,   
    case   
    when b.weight > 0 and b.weight < 50 then 50   
    else b.weight   
    end,   
    (case   
    when b.zoning = 'local' and b.weight >= 0 and b.weight <= 50 then (50 * 1)  
    when b.zoning = 'same' and b.weight >= 0 and b.weight <= 50 then (50 * 1.5)  
    when b.zoning = 'different' and b.weight >= 0 and b.weight <= 50 then (50 * 2)   
    when b.zoning = 'local' and b.weight > 50 then (b.weight * 1)  
    when b.zoning = 'same' and b.weight > 50 then (b.weight * 1.5)  
    when b.zoning = 'different' and b.weight > 50 then (b.weight * 2)  
    end)   
    ) x   
group by   
x.accountreceivingdate,x.zone,x.branch,x.ridercode,x.ridername,separationtype, dateofleaving,x.ecname,x.usertypeid,x.zoning, x.expresscentercode, x.expresscentercode  
) y   
group by y.accountreceivingdate, y.zone,y.branch,y.ridercode,y.ridername,separationtype, dateofleaving,y.ecname,y.expresscentercode,y.usertypeid,product   
)   
)y   
group by   
y.accountreceivingdate,y.zone,y.branch,y.ecname,y.ridername, y.ridercode,y.separationtype, y.dateofleaving, y.originexpresscenter,  
y.usertypeid, y.product   

/*
union all   

----- international   
select   
    x.accountreceivingdate,x.zone,x.branch,x.name ecname, x.expresscentercode originexpresscenter,x.ridername, x.ridercode, x.separationtype, x.dateofleaving,x.usertypeid,    
    product,   
    sum(cn_count) cn_count,   
case   
        when reporttype = 'cn wise' then sum(isnull(cncount,'0')) + sum(isnull(weight,'0'))   
        when reporttype = 'weight wise' then isnull(sum(cn_weight) * incentiverate,'0')   
    end  
    totalincentive  
from (   

select    
    b.accountreceivingdate,b.zone,b.branch,b.ridername,b.ridercode, separationtype, b.dateofleaving,b.usertypeid, b.zoning, ec.name,ec.expresscentercode,   
    rii.incentiverate,   
    sum(b.cn) cn_count,   
    b.weight cn_weight,   
    rii.product, rii.reporttype,  
    case when rii.reporttype = 'cn wise' then isnull(sum(b.cn) * rii.incentiverate,'0') end cncount,   
    case when rii.reporttype = 'weight wise' and sum(b.weight) >= 20 then isnull(sum(b.weight) * rii.incentiverate,'0') end weight	   
from (   
select   
    c.accountreceivingdate,z.name zone,   
    b.name branch,   
    b.branchcode,   
    r.ridercode ridercode,   
    case when r.lastname is null then r.firstname   
    when r.firstname = r.lastname then r.firstname   
    else r.firstname+' '+r.lastname end ridername,separationtype, dateofleaving,   
    case when l.attributevalue is null then r.usertypeid else l.attributevalue end usertypeid,   
    case   
    when c.orgin = c.destination then 'local'   
    when z.colorid = zz.colorid then 'same'   
    else 'different'   
    end zoning,   
    isnull(c.weight,0) weight,   
    count(c.consignmentnumber) cn,   
    c.originexpresscenter,   
    c.servicetypename   
from   
    consignment c   
    inner join branches b on c.orgin = b.branchcode   
    inner join zones z on b.zonecode = z.zonecode   
    inner join branches bb on c.destination=bb.branchcode   
    inner join zones zz on bb.zonecode = zz.zonecode   
    inner join riders r on c.ridercode = r.ridercode and c.orgin = r.branchid   
    inner join servicetypes_new st on st.servicetypename = c.servicetypename   
    left join rvdbo.lookup l on r.usertypeid = cast(l.id as varchar)   
where    
    month(c.accountreceivingdate)= '{Month}' and YEAR(c.accountreceivingdate)= '{Year}'
    and st.products = 'international'   
    and isnull(c.isapproved,'0') = '1'   
    and isnull(c.ispricecomputed,'0') = '1' and r.status = '1' AND c.expressCenterCode='{ExpressCenter}'   
    and c.consigneraccountno = '0'   
    AND c.consignmentNumber NOT IN (
             SELECT mpbc.consignmentNumber
               FROM MNP_PreBookingConsignment AS mpbc WHERE mpbc.consignmentNumber = c.consignmentNumber    
             ) 

group by   
    c.accountreceivingdate,z.name,   
    b.name,b.branchcode,c.originexpresscenter,separationtype, dateofleaving,c.servicetypename,   
    r.ridercode, c.weight,    
    case when r.lastname is null then r.firstname   
    when r.firstname = r.lastname then r.firstname   
    else r.firstname+' '+r.lastname end,   
    case when l.attributevalue is null then r.usertypeid else l.attributevalue end,   
    case when c.orgin = c.destination then 'local'   
    when z.colorid = zz.colorid then 'same' else 'different' end   
) b   
inner join expresscenters ec on b.originexpresscenter = ec.expresscentercode   
inner join mnp_retail_incentive_international rii on rii.servicetype = b.servicetypename and rii.[status] = '1'   
where    
    ec.center_type = '1'    
    and b.ridercode = '{RiderCode}'	  
group by   
    b.accountreceivingdate,b.zone,b.branch,b.ridercode,b.ridername, separationtype, b.dateofleaving,b.usertypeid, b.zoning, ec.name, ec.expresscentercode,   
    rii.product,b.weight,   
    rii.incentiverate,rii.reporttype   
) x	   
group by   
    x.accountreceivingdate,x.zone,x.branch,x.ridername, x.separationtype, x.dateofleaving,x.usertypeid, x.name, x.ridercode,x.expresscentercode, product,reporttype ,incentiverate   
*/
union all 

-- express cargo 
----- international   
select   
    x.accountreceivingdate,x.zone,x.branch,x.name ecname, x.expresscentercode originexpresscenter,x.ridername, x.ridercode, x.separationtype, x.dateofleaving,x.usertypeid,    
    product,   
    sum(cn_count) cn_count,   
    sum(x.incentiveamount)	totalincentive  
from (   

select    
    b.accountreceivingdate,b.zone,b.branch,b.ridername,b.ridercode, separationtype, b.dateofleaving,b.usertypeid, b.zoning, ec.name,ec.expresscentercode,  
--	rii.incentiverate,   
b.products product, 
    sum(b.cn) cn_count,   
    sum(case when zc.zoning = 'a' then round(b.[weight],0) * 3  
    when zc.zoning = 'b' then round(b.[weight],0) * 4 
    when zc.zoning = 'c' then round(b.[weight],0) * 0 
    when zc.zoning = 'd' then round(b.[weight],0)* 0 
    when zc.zoning = 'e' then round(b.[weight],0) * 0 end) incentiveamount 

--	b.weight cn_weight,   
--	rii.product, rii.reporttype,  
--	case when rii.reporttype = 'cn wise' then isnull(sum(b.cn) * rii.incentiverate,'0') end cncount,   
--	case when rii.reporttype = 'weight wise' and sum(b.weight) >= 20 then isnull(sum(b.weight) * rii.incentiverate,'0') end weight	   
from (   
select   
c.accountreceivingdate,z.region, 
    z.name zone,   
    b.name branch,   
    b.branchcode,   
    bb.sname dest, 
    r.ridercode ridercode,   
    case when r.lastname is null then r.firstname   
    when r.firstname = r.lastname then r.firstname   
    else r.firstname+' '+r.lastname end ridername,separationtype, dateofleaving,   
    case when l.attributevalue is null then r.usertypeid else l.attributevalue end usertypeid,   
    case   
    when c.orgin = c.destination then 'local'   
    when z.colorid = zz.colorid then 'same'   
    else 'different'   
    end zoning,   
    isnull(c.weight,0) weight,   
    count(c.consignmentnumber) cn,   
    c.originexpresscenter,   
    c.servicetypename, st.products 


from   
    consignment c   
    inner join branches b on c.orgin = b.branchcode   
    inner join zones z on b.zonecode = z.zonecode   
    inner join branches bb on c.destination=bb.branchcode   
    inner join zones zz on bb.zonecode = zz.zonecode   
    inner join riders r on c.ridercode = r.ridercode and c.orgin = r.branchid   
    inner join servicetypes_new st on st.servicetypename = c.servicetypename   
    left join rvdbo.lookup l on r.usertypeid = cast(l.id as varchar)   



where    
    month(c.accountreceivingdate)= '{Month}' and YEAR(c.accountreceivingdate)= '{Year}'                   
    and st.products = 'road n rail'   
    and isnull(c.isapproved,'0') = '1'   
    and isnull(c.ispricecomputed,'0') = '1' and r.status = '1' AND c.expressCenterCode='{ExpressCenter}'  
    and c.consigneraccountno = '0'   
    AND c.consignmentNumber NOT IN (
             SELECT mpbc.consignmentNumber
               FROM MNP_PreBookingConsignment AS mpbc WHERE mpbc.consignmentNumber = c.consignmentNumber    
             ) 

group by   
    c.accountreceivingdate,z.region, 
    z.name,   
    b.name,b.branchcode, bb.sname,c.originexpresscenter,separationtype, dateofleaving,c.servicetypename, st.products, 
    r.ridercode, c.weight,    
    case when r.lastname is null then r.firstname   
    when r.firstname = r.lastname then r.firstname   
    else r.firstname+' '+r.lastname end,   
    case when l.attributevalue is null then r.usertypeid else l.attributevalue end,   
    case when c.orgin = c.destination then 'local'   
    when z.colorid = zz.colorid then 'same' else 'different' end 
) b   
inner join expresscenters ec on b.originexpresscenter = ec.expresscentercode   
left join zoning_criteria zc on b.region+b.dest = zc.criteria 
where    
    ec.center_type = '1'    
    and b.ridercode = '{RiderCode}'	   
group by   
    b.accountreceivingdate,b.zone,b.branch,b.ridercode,b.ridername, separationtype, b.dateofleaving,b.usertypeid, b.zoning, ec.name, ec.expresscentercode , b.products  
) x	   
group by   
    x.accountreceivingdate,x.zone,x.branch,x.ridername, x.separationtype, x.dateofleaving,x.usertypeid, x.name, x.ridercode,x.expresscentercode, product 

) s    
pivot   
(   
    sum(total_incentive)   
    for product in ([domestic], [my air cargo], international, [fedex], [road n rail])   
) piv   
group by   
cast(accountreceivingdate as date),zone,branch,--ecname,originexpresscenter,  
ridername, ridercode,separationtype,dateofleaving,usertypeid  
order by accountreceivingdate,zone,branch,ridername--,ecname   ";

                await con.OpenAsync();
                var rs = await con.QueryAsync<ECBookingStaffIncentiveReportModel>(query);
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
        public async Task<List<ECBookingStaffIncentiveDetailReportModel>> GetDetailReport(ECBookingStaffIncentiveModel model)
        {
            try
            {
                string condition = "";
                string ExpressCenter = HttpContext.Current.Session["ExpressCenter"].ToString();
                string ridercondition = $@"AND R.riderCode = '{HttpContext.Current.Session["BOOKINGstaff"]}' ";
                if (model.Type == 0)
                {
                    condition = $@"CAST(C.ACCOUNTRECEIVINGDATE AS DATE) = '{model.AccountReceivingDate}'";
                }
                if (model.Type == 1)
                {
                    condition = $@" month(c.accountreceivingdate)= '{model.Month}' and YEAR(c.accountreceivingdate)= '{model.Year}'";
                }



                string query = $@" Select distinct * FROM ( 
                ----- retail incentive report
                 --- { HttpContext.Current.Session["U_NAME"]}
                -- DOMESTIC PRODUCT 
SELECT 
X.CN, 
CONVERT(VARCHAR(11),X.bookingDate,106) bookingDate, X.serviceTypeName, 
--ModifierName,--CASE WHEN ModifierName IN ('Cellophane Wrap Large Boxes','Cellophane Wrap Small Boxes'),
X.ZONE, X.BRANCH, X.ECNAME, X.ORIGINEXPRESSCENTER EC_CODE, X.RIDERCODE, X.RIDERNAME,SEPARATIONTYPE, DATEOFLEAVING, X.USERTYPEID, --X.ZONING,  
CAST(X.WEIGHT AS FLOAT) WEIGHT, 

CASE 
	WHEN X.serviceTypeName = 'Mango Fiesta 5 Kg' THEN 30   
	WHEN X.serviceTypeName = 'Mango Fiesta 7 kg' THEN 35
	WHEN X.serviceTypeName = 'Mango Fiesta 10 kg' THEN 40 
	--WHEN X.ModifierName IN ('Cellophane Wrap Small Boxes','Cellophane Wrap Large Boxes') AND X.[WEIGHT] <= '10' THEN '7' 
	--WHEN X.ModifierName IN ('Cellophane Wrap Small Boxes','Cellophane Wrap Large Boxes') AND X.[WEIGHT] > '10' THEN '10'	
	ELSE INCENTIVERATE END INCENTIVERATE, 
	
	(COUNT(X.CN) * 
	CASE 
	WHEN X.serviceTypeName = 'Mango Fiesta 5 Kg' THEN 30 
	WHEN X.serviceTypeName = 'Mango Fiesta 7 kg' THEN  35 
	WHEN X.serviceTypeName = 'Mango Fiesta 10 kg' THEN 40 
	--WHEN X.ModifierName IN ('Cellophane Wrap Small Boxes','Cellophane Wrap Large Boxes') AND X.[WEIGHT] <= '10' THEN '7' * SUM(x.pieces)
	--WHEN X.ModifierName IN ('Cellophane Wrap Small Boxes','Cellophane Wrap Large Boxes') AND X.[WEIGHT] > '10' THEN '10' * SUM(x.pieces)
	ELSE INCENTIVERATE END
	) TOTALINCENTIVE 
FROM ( 
SELECT 
	B.CN, 
	B.bookingDate, B.serviceTypeName, 
	B.ZONE, B.BRANCH,B.BRANCHCODE, B.ORIGINEXPRESSCENTER, B.RIDERCODE, B.RIDERNAME, SEPARATIONTYPE, B.DATEOFLEAVING,B.USERTYPEID, B.ZONING, EC.NAME ECNAME, --ModifierName,pieces,
  CAST(CASE WHEN B.WEIGHT > 0 AND B.WEIGHT <= 0.5 THEN 0.5 
     WHEN B.WEIGHT > 0.5 AND B.WEIGHT <= 1.0 THEN 1 
     WHEN B.WEIGHT > 1.0 AND B.WEIGHT <= 1.5 THEN 1.5 
     WHEN B.WEIGHT > 1.5 AND B.WEIGHT <= 2.0 THEN 2 
     WHEN B.WEIGHT > 2.0 AND B.WEIGHT <= 3.0 THEN 3 
     WHEN B.WEIGHT > 3.0 AND B.WEIGHT <= 4.0 THEN 4 
     WHEN B.WEIGHT > 4.0 AND B.WEIGHT <= 5.0 THEN 5 
     WHEN B.WEIGHT > 5.0 AND B.WEIGHT <= 6.0 THEN 6 
     WHEN B.WEIGHT > 6.0 AND B.WEIGHT <= 7.0 THEN 7 
     WHEN B.WEIGHT > 7.0 AND B.WEIGHT <= 8.0 THEN 8 
     WHEN B.WEIGHT > 8.0 AND B.WEIGHT <= 9.0 THEN 9 
     WHEN B.WEIGHT > 9.0 AND B.WEIGHT <= 10.0 THEN 10 
     WHEN B.WEIGHT > 10.0 AND B.WEIGHT <= 11.0 THEN 11 
     WHEN B.WEIGHT > 11.0 AND B.WEIGHT <= 12.0 THEN 12 
     WHEN B.WEIGHT > 12.0 AND B.WEIGHT <= 13.0 THEN 13 
     WHEN B.WEIGHT > 13.0 AND B.WEIGHT <= 14.0 THEN 14 
     WHEN B.WEIGHT > 14.0 AND B.WEIGHT <= 15.0 THEN 15 
     WHEN B.WEIGHT > 15.0 AND B.WEIGHT <= 16.0 THEN 16 
     WHEN B.WEIGHT > 16.0 AND B.WEIGHT <= 17.0 THEN 17 
     WHEN B.WEIGHT > 17.0 AND B.WEIGHT <= 18.0 THEN 18 
     WHEN B.WEIGHT > 18.0 AND B.WEIGHT <= 19.0 THEN 19 
     WHEN B.WEIGHT > 19.0 AND B.WEIGHT <= 20.0 THEN 20 
     WHEN B.WEIGHT > 20.0 AND B.WEIGHT <= 21.0 THEN 21 
     WHEN B.WEIGHT > 21.0 AND B.WEIGHT <= 22.0 THEN 22 
     WHEN B.WEIGHT > 22.0 AND B.WEIGHT <= 23.0 THEN 23 
     WHEN B.WEIGHT > 23.0 AND B.WEIGHT <= 24.0 THEN 24 
     WHEN B.WEIGHT > 24.0 AND B.WEIGHT <= 25.0 THEN 25 
     WHEN B.WEIGHT > 25.0 AND B.WEIGHT <= 26.0 THEN 26 
     WHEN B.WEIGHT > 26.0 AND B.WEIGHT <= 27.0 THEN 27 
     WHEN B.WEIGHT > 27.0 AND B.WEIGHT <= 28.0 THEN 28 
     WHEN B.WEIGHT > 28.0 AND B.WEIGHT <= 29.0 THEN 29 
     WHEN B.WEIGHT > 29.0 AND B.WEIGHT <= 30.0 THEN 30 
     WHEN B.WEIGHT > 30.0 AND B.WEIGHT <= 31.0 THEN 31 
     WHEN B.WEIGHT > 31.0 AND B.WEIGHT <= 32.0 THEN 32 
     WHEN B.WEIGHT > 32.0 AND B.WEIGHT <= 33.0 THEN 33 
     WHEN B.WEIGHT > 33.0 AND B.WEIGHT <= 34.0 THEN 34 
     WHEN B.WEIGHT > 34.0 AND B.WEIGHT <= 35.0 THEN 35 
     WHEN B.WEIGHT > 35.0 AND B.WEIGHT <= 36.0 THEN 36 
     WHEN B.WEIGHT > 36.0 AND B.WEIGHT <= 37.0 THEN 37 
     WHEN B.WEIGHT > 37.0 AND B.WEIGHT <= 38.0 THEN 38 
     WHEN B.WEIGHT > 38.0 AND B.WEIGHT <= 39.0 THEN 39 
     WHEN B.WEIGHT > 39.0 AND B.WEIGHT <= 40.0 THEN 40 
     WHEN B.WEIGHT > 40.0 AND B.WEIGHT <= 41.0 THEN 41 
     WHEN B.WEIGHT > 41.0 AND B.WEIGHT <= 42.0 THEN 42 
     WHEN B.WEIGHT > 42.0 AND B.WEIGHT <= 43.0 THEN 43 
     WHEN B.WEIGHT > 43.0 AND B.WEIGHT <= 44.0 THEN 44 
     WHEN B.WEIGHT > 44.0 AND B.WEIGHT <= 45.0 THEN 45 
     WHEN B.WEIGHT > 45.0 AND B.WEIGHT <= 46.0 THEN 46 
     WHEN B.WEIGHT > 46.0 AND B.WEIGHT <= 47.0 THEN 47 
     WHEN B.WEIGHT > 47.0 AND B.WEIGHT <= 48.0 THEN 48 
     WHEN B.WEIGHT > 48.0 AND B.WEIGHT <= 49.0 THEN 49 
     WHEN B.WEIGHT > 49.0 THEN 50 
  END AS varchar) WEIGHT 
FROM ( 
SELECT DISTINCT
	Z.NAME ZONE, 
	B.NAME BRANCH,B.BRANCHCODE,C.ORIGINEXPRESSCENTER, 
	R.RIDERCODE RIDERCODE, 
	CASE WHEN R.LASTNAME IS NULL THEN R.FIRSTNAME 
	WHEN R.FIRSTNAME = R.LASTNAME THEN R.FIRSTNAME 
	ELSE R.FIRSTNAME+' '+R.LASTNAME END RIDERNAME, 
	CASE WHEN L.AttributeValue IS NULL THEN R.userTypeId ELSE L.AttributeValue END USERTYPEID, 
	CASE 
	WHEN C.ORGIN = C.DESTINATION THEN 'LOCAL' 
	WHEN Z.COLORID = ZZ.COLORID THEN 'SAME' 
	ELSE 'DIFFERENT' 
	END ZONING, 
	ISNULL(C.WEIGHT,0) WEIGHT, 
	C.CONSIGNMENTNUMBER CN, R.SEPARATIONTYPE, R.DATEOFLEAVING, 
	C.bookingDate, C.serviceTypeName
	--PM.name ModifierName, c.pieces
FROM 
    CONSIGNMENT C    
    INNER JOIN BRANCHES B ON C.ORGIN = B.BRANCHCODE 
    INNER JOIN ZONES Z ON B.ZONECODE = Z.ZONECODE 
    INNER JOIN BRANCHES BB ON C.DESTINATION=BB.BRANCHCODE 
    INNER JOIN ZONES ZZ ON BB.ZONECODE = ZZ.ZONECODE 
    INNER JOIN RIDERS R ON C.RIDERCODE = R.RIDERCODE AND C.ORGIN = R.BRANCHID 
    INNER JOIN SERVICETYPES_NEW ST ON ST.SERVICETYPENAME = C.SERVICETYPENAME 
	LEFT JOIN rvdbo.Lookup l ON R.userTypeId = CAST(L.Id AS VARCHAR)  
    --LEFT JOIN ConsignmentModifier cm ON cm.consignmentNumber = c.consignmentNumber
    --LEFT JOIN PriceModifiers AS pm ON pm.id = cm.priceModifierId AND pm.id IN ('111','112')
WHERE  
		{condition}
		AND ST.PRODUCTS = 'DOMESTIC'
		AND ISNULL(C.ISAPPROVED,'0') = '1'  
		AND ISNULL(C.ISPRICECOMPUTED,'0') = '1' 
		AND (C.CONSIGNERACCOUNTNO = '0' OR C.consignerAccountNo LIKE '%CC%')
		{ridercondition}
		and r.status = '1'
		AND c.expressCenterCode='{ExpressCenter}'   
        AND c.consignmentNumber NOT IN (
             SELECT mpbc.consignmentNumber
               FROM MNP_PreBookingConsignment AS mpbc WHERE mpbc.consignmentNumber = c.consignmentNumber    
             ) 

GROUP BY 
    Z.NAME,C.CONSIGNMENTNUMBER, 
    B.NAME,B.BRANCHCODE,C.ORIGINEXPRESSCENTER, 
    R.RIDERCODE, C.WEIGHT, c.pieces,
	C.bookingDate, C.serviceTypeName, 
    CASE WHEN R.LASTNAME IS NULL THEN R.FIRSTNAME 
    WHEN R.FIRSTNAME = R.LASTNAME THEN R.FIRSTNAME 
    ELSE R.FIRSTNAME+' '+R.LASTNAME END, 
	CASE WHEN L.AttributeValue IS NULL THEN R.userTypeId ELSE L.AttributeValue END, 
    CASE WHEN C.ORGIN = C.DESTINATION THEN 'LOCAL' 
    WHEN Z.COLORID = ZZ.COLORID THEN 'SAME' ELSE 'DIFFERENT' END, R.SEPARATIONTYPE, R.DATEOFLEAVING--, PM.name  
    ) B 
LEFT JOIN EXPRESSCENTERS EC ON B.ORIGINEXPRESSCENTER = EC.EXPRESSCENTERCODE 
WHERE  
--B.ZONING = 'DIFFERENT' 
EC.CENTER_TYPE = '1'  
GROUP BY 
B.CN, 
	B.bookingDate, B.serviceTypeName, 
B.ZONE, B.BRANCH,B.BRANCHCODE, EC.NAME,B.ORIGINEXPRESSCENTER, B.RIDERCODE, B.RIDERNAME, SEPARATIONTYPE, B.DATEOFLEAVING,B.USERTYPEID, B.ZONING,-- ModifierName,pieces,
  CASE WHEN B.WEIGHT > 0 AND B.WEIGHT <= 0.5 THEN 0.5 
     WHEN B.WEIGHT > 0.5 AND B.WEIGHT <= 1.0 THEN 1 
     WHEN B.WEIGHT > 1.0 AND B.WEIGHT <= 1.5 THEN 1.5 
     WHEN B.WEIGHT > 1.5 AND B.WEIGHT <= 2.0 THEN 2 
     WHEN B.WEIGHT > 2.0 AND B.WEIGHT <= 3.0 THEN 3 
     WHEN B.WEIGHT > 3.0 AND B.WEIGHT <= 4.0 THEN 4 
     WHEN B.WEIGHT > 4.0 AND B.WEIGHT <= 5.0 THEN 5 
     WHEN B.WEIGHT > 5.0 AND B.WEIGHT <= 6.0 THEN 6 
     WHEN B.WEIGHT > 6.0 AND B.WEIGHT <= 7.0 THEN 7 
     WHEN B.WEIGHT > 7.0 AND B.WEIGHT <= 8.0 THEN 8 
     WHEN B.WEIGHT > 8.0 AND B.WEIGHT <= 9.0 THEN 9 
     WHEN B.WEIGHT > 9.0 AND B.WEIGHT <= 10.0 THEN 10 
     WHEN B.WEIGHT > 10.0 AND B.WEIGHT <= 11.0 THEN 11 
     WHEN B.WEIGHT > 11.0 AND B.WEIGHT <= 12.0 THEN 12 
     WHEN B.WEIGHT > 12.0 AND B.WEIGHT <= 13.0 THEN 13 
     WHEN B.WEIGHT > 13.0 AND B.WEIGHT <= 14.0 THEN 14 
     WHEN B.WEIGHT > 14.0 AND B.WEIGHT <= 15.0 THEN 15 
     WHEN B.WEIGHT > 15.0 AND B.WEIGHT <= 16.0 THEN 16 
     WHEN B.WEIGHT > 16.0 AND B.WEIGHT <= 17.0 THEN 17 
     WHEN B.WEIGHT > 17.0 AND B.WEIGHT <= 18.0 THEN 18 
     WHEN B.WEIGHT > 18.0 AND B.WEIGHT <= 19.0 THEN 19 
     WHEN B.WEIGHT > 19.0 AND B.WEIGHT <= 20.0 THEN 20 
     WHEN B.WEIGHT > 20.0 AND B.WEIGHT <= 21.0 THEN 21 
     WHEN B.WEIGHT > 21.0 AND B.WEIGHT <= 22.0 THEN 22 
     WHEN B.WEIGHT > 22.0 AND B.WEIGHT <= 23.0 THEN 23 
     WHEN B.WEIGHT > 23.0 AND B.WEIGHT <= 24.0 THEN 24 
     WHEN B.WEIGHT > 24.0 AND B.WEIGHT <= 25.0 THEN 25 
     WHEN B.WEIGHT > 25.0 AND B.WEIGHT <= 26.0 THEN 26 
     WHEN B.WEIGHT > 26.0 AND B.WEIGHT <= 27.0 THEN 27 
     WHEN B.WEIGHT > 27.0 AND B.WEIGHT <= 28.0 THEN 28 
     WHEN B.WEIGHT > 28.0 AND B.WEIGHT <= 29.0 THEN 29 
     WHEN B.WEIGHT > 29.0 AND B.WEIGHT <= 30.0 THEN 30 
     WHEN B.WEIGHT > 30.0 AND B.WEIGHT <= 31.0 THEN 31 
     WHEN B.WEIGHT > 31.0 AND B.WEIGHT <= 32.0 THEN 32 
     WHEN B.WEIGHT > 32.0 AND B.WEIGHT <= 33.0 THEN 33 
     WHEN B.WEIGHT > 33.0 AND B.WEIGHT <= 34.0 THEN 34 
     WHEN B.WEIGHT > 34.0 AND B.WEIGHT <= 35.0 THEN 35 
     WHEN B.WEIGHT > 35.0 AND B.WEIGHT <= 36.0 THEN 36 
     WHEN B.WEIGHT > 36.0 AND B.WEIGHT <= 37.0 THEN 37 
     WHEN B.WEIGHT > 37.0 AND B.WEIGHT <= 38.0 THEN 38 
     WHEN B.WEIGHT > 38.0 AND B.WEIGHT <= 39.0 THEN 39 
     WHEN B.WEIGHT > 39.0 AND B.WEIGHT <= 40.0 THEN 40 
     WHEN B.WEIGHT > 40.0 AND B.WEIGHT <= 41.0 THEN 41 
     WHEN B.WEIGHT > 41.0 AND B.WEIGHT <= 42.0 THEN 42 
     WHEN B.WEIGHT > 42.0 AND B.WEIGHT <= 43.0 THEN 43 
     WHEN B.WEIGHT > 43.0 AND B.WEIGHT <= 44.0 THEN 44 
     WHEN B.WEIGHT > 44.0 AND B.WEIGHT <= 45.0 THEN 45 
     WHEN B.WEIGHT > 45.0 AND B.WEIGHT <= 46.0 THEN 46 
     WHEN B.WEIGHT > 46.0 AND B.WEIGHT <= 47.0 THEN 47 
     WHEN B.WEIGHT > 47.0 AND B.WEIGHT <= 48.0 THEN 48 
     WHEN B.WEIGHT > 48.0 AND B.WEIGHT <= 49.0 THEN 49 
     WHEN B.WEIGHT > 49.0 THEN 50 
  END 
  ) X 
  INNER JOIN MNP_RIDERINCENTIVERATES RIR ON X.WEIGHT = CAST(RIR.WEIGHT as float) AND X.ZONING = RIR.INCENTIVETYPE 
  
  
  
GROUP BY 
X.CN,X.bookingDate, X.serviceTypeName, --ModifierName,
X.ZONE, X.BRANCH, X.ECNAME,X.ORIGINEXPRESSCENTER, X.RIDERCODE, X.RIDERNAME, SEPARATIONTYPE, DATEOFLEAVING,X.USERTYPEID, --X.ZONING,  
X.WEIGHT,  INCENTIVERATE 
 
 
UNION ALL 
 
-- MY AIR CARGO 
SELECT 
	B.CN, 
	CONVERT(VARCHAR(11),B.bookingDate,106) bookingDate, B.serviceTypeName, --NULL ModifierName,
	B.ZONE,B.BRANCH, EC.NAME ECNAME, EC.EXPRESSCENTERCODE EC_CODE, B.RIDERCODE,B.RIDERNAME, SEPARATIONTYPE, B.DATEOFLEAVING,B.USERTYPEID,   
	CASE 
	WHEN B.WEIGHT > 0 AND B.WEIGHT < 50 THEN 50 
	ELSE B.WEIGHT 
	END WEIGHT ,
	CASE  
	WHEN B.ZONING = 'LOCAL' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN 50  
	WHEN B.ZONING = 'SAME' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN 50  
	WHEN B.ZONING = 'DIFFERENT' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN 50  
	WHEN B.ZONING = 'LOCAL' AND B.WEIGHT > 50 THEN 1   
	WHEN B.ZONING = 'SAME' AND B.WEIGHT > 50 THEN 1.5   
	WHEN B.ZONING = 'DIFFERENT' AND B.WEIGHT > 50 THEN 2  
	END INCENTIVERATE ,
	(CASE   
	WHEN B.ZONING = 'LOCAL' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN (B.WEIGHT * 1)   
	WHEN B.ZONING = 'SAME' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN (B.WEIGHT * 1.5)   
	WHEN B.ZONING = 'DIFFERENT' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN (B.WEIGHT * 2)   
	WHEN B.ZONING = 'LOCAL' AND B.WEIGHT > 50 THEN (B.WEIGHT * 1)   
	WHEN B.ZONING = 'SAME' AND B.WEIGHT > 50 THEN (B.WEIGHT * 1.5)   
	WHEN B.ZONING = 'DIFFERENT' AND B.WEIGHT > 50 THEN (B.WEIGHT * 2)   
	END) INCENTIVERATE 
FROM 
( 
SELECT 
	Z.NAME ZONE, 
	B.NAME BRANCH, 
	B.BRANCHCODE, 
	R.RIDERCODE RIDERCODE, 
	CASE WHEN R.LASTNAME IS NULL THEN R.FIRSTNAME 
	WHEN R.FIRSTNAME = R.LASTNAME THEN R.FIRSTNAME 
	ELSE R.FIRSTNAME+' '+R.LASTNAME END RIDERNAME,SEPARATIONTYPE, DATEOFLEAVING, 
	CASE WHEN L.AttributeValue IS NULL THEN R.userTypeId ELSE L.AttributeValue END USERTYPEID, 
	CASE 
	WHEN C.ORGIN = C.DESTINATION THEN 'LOCAL' 
	WHEN Z.COLORID = ZZ.COLORID THEN 'SAME' 
	ELSE 'DIFFERENT' 
	END ZONING, 
	ISNULL(C.WEIGHT,0) WEIGHT, 
	C.CONSIGNMENTNUMBER CN, 
	'5' INCENTIVERATE, 
	C.originExpressCenter, 
	C.bookingDate, C.serviceTypeName 
FROM 
	CONSIGNMENT C 
	INNER JOIN BRANCHES B ON C.ORGIN = B.BRANCHCODE 
	INNER JOIN ZONES Z ON B.ZONECODE = Z.ZONECODE 
	INNER JOIN BRANCHES BB ON C.DESTINATION=BB.BRANCHCODE 
	INNER JOIN ZONES ZZ ON BB.ZONECODE = ZZ.ZONECODE 
	INNER JOIN RIDERS R ON C.RIDERCODE = R.RIDERCODE AND C.ORGIN = R.BRANCHID 
	--INNER JOIN MNP_RIDERINCENTIVERATES RIR ON C.WEIGHT = RIR.WEIGHT 
	INNER JOIN SERVICETYPES_NEW ST ON ST.SERVICETYPENAME = C.SERVICETYPENAME 
	LEFT JOIN rvdbo.Lookup l ON R.userTypeId = CAST(L.Id AS VARCHAR) 
WHERE  
	{condition}
	AND ST.PRODUCTS = 'DOMESTIC' AND ST.SERVICETYPENAME = 'MY AIR CARGO' 
	AND ISNULL(C.ISAPPROVED,'0') = '1' 
	AND ISNULL(C.ISPRICECOMPUTED,'0') = '1' 
	AND C.CONSIGNERACCOUNTNO = '0' 
    {ridercondition}
    AND c.consignmentNumber NOT IN (
             SELECT mpbc.consignmentNumber
               FROM MNP_PreBookingConsignment AS mpbc WHERE mpbc.consignmentNumber = c.consignmentNumber    
             ) 


GROUP BY 
	Z.NAME,C.CONSIGNMENTNUMBER, 
	C.bookingDate, C.serviceTypeName, 
	B.NAME,B.BRANCHCODE,C.originExpressCenter,SEPARATIONTYPE, DATEOFLEAVING, 
	R.RIDERCODE, C.WEIGHT,  
	CASE WHEN R.LASTNAME IS NULL THEN R.FIRSTNAME 
	WHEN R.FIRSTNAME = R.LASTNAME THEN R.FIRSTNAME 
	ELSE R.FIRSTNAME+' '+R.LASTNAME END, 
	CASE WHEN L.AttributeValue IS NULL THEN R.userTypeId ELSE L.AttributeValue END, 
	CASE WHEN C.ORGIN = C.DESTINATION THEN 'LOCAL' 
	WHEN Z.COLORID = ZZ.COLORID THEN 'SAME' ELSE 'DIFFERENT' END 
) B 
	LEFT JOIN EXPRESSCENTERS EC ON B.originExpressCenter = EC.EXPRESSCENTERCODE 
WHERE  
	EC.CENTER_TYPE = '1'  
GROUP BY 
	B.CN,B.bookingDate, B.serviceTypeName, 
	B.ZONE,B.BRANCH,B.RIDERCODE,B.RIDERNAME, SEPARATIONTYPE, B.DATEOFLEAVING,B.USERTYPEID, B.ZONING, B.INCENTIVERATE,EC.NAME,EC.EXPRESSCENTERCODE, 
	CASE 
	WHEN B.WEIGHT > 0 AND B.WEIGHT < 50 THEN 50 
	ELSE B.WEIGHT 
	END, 
	CASE  
	WHEN B.ZONING = 'LOCAL' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN 50  
	WHEN B.ZONING = 'SAME' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN 50  
	WHEN B.ZONING = 'DIFFERENT' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN 50  
	WHEN B.ZONING = 'LOCAL' AND B.WEIGHT > 50 THEN 1   
	WHEN B.ZONING = 'SAME' AND B.WEIGHT > 50 THEN 1.5   
	WHEN B.ZONING = 'DIFFERENT' AND B.WEIGHT > 50 THEN 2  
	END,  
	(CASE   
	WHEN B.ZONING = 'LOCAL' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN (B.WEIGHT * 1)   
	WHEN B.ZONING = 'SAME' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN (B.WEIGHT * 1.5)   
	WHEN B.ZONING = 'DIFFERENT' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN (B.WEIGHT * 2)   
	WHEN B.ZONING = 'LOCAL' AND B.WEIGHT > 50 THEN (B.WEIGHT * 1)   
	WHEN B.ZONING = 'SAME' AND B.WEIGHT > 50 THEN (B.WEIGHT * 1.5)   
	WHEN B.ZONING = 'DIFFERENT' AND B.WEIGHT > 50 THEN (B.WEIGHT * 2)   
	END)  
	
	
	
UNION ALL 
 
-- EXPRESS CARGO
SELECT 
	B.CN, 
	CONVERT(VARCHAR(11),B.bookingDate,106) bookingDate, B.serviceTypeName, --NULL ModifierName,
	B.ZONE,B.BRANCH, EC.NAME ECNAME, EC.EXPRESSCENTERCODE EC_CODE, B.RIDERCODE,B.RIDERNAME, SEPARATIONTYPE, B.DATEOFLEAVING,B.USERTYPEID,   
	CASE 
	WHEN B.WEIGHT > 0 AND B.WEIGHT < 50 THEN 50 
	ELSE B.WEIGHT 
	END WEIGHT ,
	CASE  
	WHEN B.ZONING = 'LOCAL' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN 50  
	WHEN B.ZONING = 'SAME' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN 50  
	WHEN B.ZONING = 'DIFFERENT' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN 50  
	WHEN B.ZONING = 'LOCAL' AND B.WEIGHT > 50 THEN 1   
	WHEN B.ZONING = 'SAME' AND B.WEIGHT > 50 THEN 1.5   
	WHEN B.ZONING = 'DIFFERENT' AND B.WEIGHT > 50 THEN 2  
	END INCENTIVERATE ,
	sum(case when zc.zoning = 'a' then round(b.[weight],0) * 3  
    when zc.zoning = 'b' then round(b.[weight],0) * 4 
    when zc.zoning = 'c' then round(b.[weight],0) * 0 
    when zc.zoning = 'd' then round(b.[weight],0)* 0 
    when zc.zoning = 'e' then round(b.[weight],0) * 0 end) INCENTIVERATE 
FROM 
( 
SELECT 
	Z.NAME ZONE, z.region, 
	B.NAME BRANCH, bb.sname dest, 
	B.BRANCHCODE, 
	R.RIDERCODE RIDERCODE, 
	CASE WHEN R.LASTNAME IS NULL THEN R.FIRSTNAME 
	WHEN R.FIRSTNAME = R.LASTNAME THEN R.FIRSTNAME 
	ELSE R.FIRSTNAME+' '+R.LASTNAME END RIDERNAME,SEPARATIONTYPE, DATEOFLEAVING, 
	CASE WHEN L.AttributeValue IS NULL THEN R.userTypeId ELSE L.AttributeValue END USERTYPEID, 
	CASE 
	WHEN C.ORGIN = C.DESTINATION THEN 'LOCAL' 
	WHEN Z.COLORID = ZZ.COLORID THEN 'SAME' 
	ELSE 'DIFFERENT' 
	END ZONING, 
	ISNULL(C.WEIGHT,0) WEIGHT, 
	C.CONSIGNMENTNUMBER CN, 
	0 INCENTIVERATE, 
	C.originExpressCenter, 
	C.bookingDate, C.serviceTypeName 
FROM 
	CONSIGNMENT C 
	INNER JOIN BRANCHES B ON C.ORGIN = B.BRANCHCODE 
	INNER JOIN ZONES Z ON B.ZONECODE = Z.ZONECODE 
	INNER JOIN BRANCHES BB ON C.DESTINATION=BB.BRANCHCODE 
	INNER JOIN ZONES ZZ ON BB.ZONECODE = ZZ.ZONECODE 
	INNER JOIN RIDERS R ON C.RIDERCODE = R.RIDERCODE AND C.ORGIN = R.BRANCHID 
	--INNER JOIN MNP_RIDERINCENTIVERATES RIR ON C.WEIGHT = RIR.WEIGHT 
	INNER JOIN SERVICETYPES_NEW ST ON ST.SERVICETYPENAME = C.SERVICETYPENAME 
	LEFT JOIN rvdbo.Lookup l ON R.userTypeId = CAST(L.Id AS VARCHAR) 
WHERE  
	{condition}
	and st.products = 'road n rail'
	AND ISNULL(C.ISAPPROVED,'0') = '1' 
	AND ISNULL(C.ISPRICECOMPUTED,'0') = '1' 
	AND C.CONSIGNERACCOUNTNO = '0' 
   {ridercondition}
    AND c.consignmentNumber NOT IN (
             SELECT mpbc.consignmentNumber
               FROM MNP_PreBookingConsignment AS mpbc WHERE mpbc.consignmentNumber = c.consignmentNumber    
             ) 


GROUP BY 
	Z.NAME,C.CONSIGNMENTNUMBER, z.region, bb.sname,
	C.bookingDate, C.serviceTypeName, 
	B.NAME,B.BRANCHCODE,C.originExpressCenter,SEPARATIONTYPE, DATEOFLEAVING, 
	R.RIDERCODE, C.WEIGHT,  
	CASE WHEN R.LASTNAME IS NULL THEN R.FIRSTNAME 
	WHEN R.FIRSTNAME = R.LASTNAME THEN R.FIRSTNAME 
	ELSE R.FIRSTNAME+' '+R.LASTNAME END, 
	CASE WHEN L.AttributeValue IS NULL THEN R.userTypeId ELSE L.AttributeValue END, 
	CASE WHEN C.ORGIN = C.DESTINATION THEN 'LOCAL' 
	WHEN Z.COLORID = ZZ.COLORID THEN 'SAME' ELSE 'DIFFERENT' END 
) B 
	LEFT JOIN EXPRESSCENTERS EC ON B.originExpressCenter = EC.EXPRESSCENTERCODE 
	left join zoning_criteria zc on b.region+b.dest = zc.criteria 
WHERE  
	EC.CENTER_TYPE = '1'  
GROUP BY 
	B.CN,B.bookingDate, B.serviceTypeName, 
	B.ZONE,B.BRANCH,B.RIDERCODE,B.RIDERNAME, SEPARATIONTYPE, B.DATEOFLEAVING,B.USERTYPEID, B.ZONING, B.INCENTIVERATE,EC.NAME,EC.EXPRESSCENTERCODE, 
	CASE 
	WHEN B.WEIGHT > 0 AND B.WEIGHT < 50 THEN 50 
	ELSE B.WEIGHT 
	END, 
	CASE  
	WHEN B.ZONING = 'LOCAL' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN 50  
	WHEN B.ZONING = 'SAME' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN 50  
	WHEN B.ZONING = 'DIFFERENT' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN 50  
	WHEN B.ZONING = 'LOCAL' AND B.WEIGHT > 50 THEN 1   
	WHEN B.ZONING = 'SAME' AND B.WEIGHT > 50 THEN 1.5   
	WHEN B.ZONING = 'DIFFERENT' AND B.WEIGHT > 50 THEN 2  
	END,  
	(CASE   
	WHEN B.ZONING = 'LOCAL' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN (B.WEIGHT * 1)   
	WHEN B.ZONING = 'SAME' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN (B.WEIGHT * 1.5)   
	WHEN B.ZONING = 'DIFFERENT' AND B.WEIGHT >= 0 AND B.WEIGHT <= 50 THEN (B.WEIGHT * 2)   
	WHEN B.ZONING = 'LOCAL' AND B.WEIGHT > 50 THEN (B.WEIGHT * 1)   
	WHEN B.ZONING = 'SAME' AND B.WEIGHT > 50 THEN (B.WEIGHT * 1.5)   
	WHEN B.ZONING = 'DIFFERENT' AND B.WEIGHT > 50 THEN (B.WEIGHT * 2)   
	END)  	
	 
	 	 
) W 
--ORDER BY 	 
--1,4,5,2,3 ASC";
                await con.OpenAsync();
                var rs = await con.QueryAsync<ECBookingStaffIncentiveDetailReportModel>(query);
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