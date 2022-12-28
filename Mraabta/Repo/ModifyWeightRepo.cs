using Dapper;
using MRaabta.Files;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MRaabta.Files.TrackingModel;

namespace MRaabta.Repo
{
    public class ModifyWeightRepo : GeneralRepo
    {
        SqlConnection con;
        public ModifyWeightRepo()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            SetCon(con);
        }
        public async Task OpenAsync()
        {
            await con.OpenAsync();
        }
        public void Close()
        {
            con.Close();
        }

        #region Weight
        public async Task<List<WeightDisplayModel>> WeightDetail(WeightModel model, UserModel u)
        {
            try
            {
                var query = $@" SELECT c.* INTO #WEIGHT_DISCREPANCY__2618 FROM Consignment c 
                        INNER JOIN CreditClients cc ON cc.id = c.creditClientId AND cc.IndustryId != '67' 
                        WHERE CAST(c.bookingDate AS DATE) >= '{model.From}' and isnull(c.cod,'0') in ('0','1') 

	                        SELECT DISTINCT b.bookingrider,
	                        b.consignmentNumber, CONVERT(VARCHAR(11),b.bookingDate,106) BOOKINGDATE,  
	                        orignZone,orignBranch, destinationBranch,destinationZone , b.serviceTypeName,  
	                        cc.name clientname, cc.accountNo,  
	                        b.CNpieces, b.Pieces Ops_Pieces,
	                        b.CNWeight, b.[WEIGHT] Ops_weight,
	                        b.[WEIGHT] - b.CNWeight Weight_diff, 
	                        b.ops, b.Number Ops_Number, B.LOCATION, 
	                        CASE WHEN isnull(b.isApproved,'0') = '1' THEN 'YES' ELSE 'NO' END isApproved, 
	                        CASE WHEN isnull(b.isPriceComputed,'0') = '1' THEN 'YES' ELSE 'NO' END isPriceComputed, 
	                        CASE WHEN isnull(b.IsInvoiced,'0') = '1' THEN 'YES' ELSE 'NO' END IsInvoiced, 
	                        CASE WHEN isnull(b.COD,'0') = '1' THEN 'COD' ELSE '' END CODStatus 
                        FROM (
		                        SELECT  c.riderCode bookingrider,
		                        c.consignmentNumber, CONVERT(VARCHAR(11),c.bookingDate,106) BOOKINGDATE,  
		                        z.name orignZone,b1.name orignBranch, b2.name destinationBranch,z1.name destinationZone , c.serviceTypeName,  
		                        c.consignerAccountNo, c.creditClientId, c.isApproved, 
		                        c.isPriceComputed, c.IsInvoiced, c.COD, 
		                        isnull(c.pieces,'0') CNpieces, isnull(c.[weight],'0') CNWeight, 
		                        'MANIFEST' OPS, isnull(mc.Pieces,'0') Pieces, 
		                        cast(isnull(mc.[Weight],'0') AS VARCHAR) WEIGHT, 
		                        mc.manifestNumber Number, BB.name LOCATION 
		                        FROM #WEIGHT_DISCREPANCY__2618 C 
		                        INNER JOIN Mnp_ConsignmentManifest mc ON c.consignmentNumber = mc.consignmentNumber 
		                        INNER JOIN Mnp_Manifest mm ON MC.manifestNumber = MM.manifestNumber 
		                        INNER JOIN Branches BB ON MM.branchCode = BB.branchCode 
		                        INNER JOIN Branches b1 ON c.orgin = b1.branchCode 
		                        INNER JOIN Branches b2 ON c.destination = b2.branchCode 
		                        INNER JOIN Zones z ON b1.zoneCode = z.zoneCode 
		                        INNER JOIN Zones z1 ON b2.zoneCode = z1.zoneCode 
		                        WHERE (cast(isnull(mc.[Weight],'0') AS float) - cast(c.[weight] AS float) ) >= '{model.Weight}'
                                { (model.BookingCode != null || model.BookingCode != "" ? " " : $"AND C.riderCode = '{model.BookingCode}' ")} 
		                         AND Z.ZONECODE = '{u.ZoneCode}' AND b1.branchCode = '{u.BranchCode}'
	                        UNION ALL  
		                        SELECT c.riderCode bookingrider, 
		                        c.consignmentNumber, CONVERT(VARCHAR(11),c.bookingDate,106) BOOKINGDATE,  
		                        z.name orignZone,b1.name orignBranch, b2.name destinationBranch,z1.name destinationZone , c.serviceTypeName,   
		                        c.consignerAccountNo, c.creditClientId, c.isApproved, 
		                        c.isPriceComputed, c.IsInvoiced, c.COD,
		                        isnull(c.pieces,'0') CNpieces, isnull(c.[weight],'0') CNWeight, 
		                        'LOADING' OPS, isnull(ml.cnPieces,'0') Pieces, 
		                        isnull(ml.CNWeight,'0') WEIGHT, Cast(isnull(ml.loadingId,'0') AS VARCHAR) number, 
		                        BB.name LOCATION
		                        FROM #WEIGHT_DISCREPANCY__2618 C 
		                        INNER JOIN MnP_LoadingConsignment ml ON c.consignmentNumber = ml.consignmentNumber  
		                        INNER JOIN MnP_Loading L ON ML.loadingId = L.id 
		                        INNER JOIN Branches bB ON L.branchCode = BB.branchCode 
		                        INNER JOIN Branches b1 ON c.orgin = b1.branchCode 
		                        INNER JOIN Branches b2 ON c.destination = b2.branchCode 
		                        INNER JOIN Zones z ON b1.zoneCode = z.zoneCode 
		                        INNER JOIN Zones z1 ON b2.zoneCode = z1.zoneCode 
		                        WHERE (convert(float,ml.CNWeight) - c.[weight]) >=  '{model.Weight}'
                                { (model.BookingCode != null || model.BookingCode != "" ? " " : $"AND C.riderCode = '{model.BookingCode}' ")} 
		                        AND ISNUMERIC(ml.CNWeight)='1' AND Z.ZONECODE = '{u.ZoneCode}' AND b1.branchCode = '{u.BranchCode}'
	                        UNION ALL  
		                        SELECT c.riderCode bookingrider, 
		                        c.consignmentNumber, CONVERT(VARCHAR(11),c.bookingDate,106) BOOKINGDATE,  
		                        z.name orignZone,b1.name orignBranch, b2.name destinationBranch,z1.name destinationZone , 
		                        c.serviceTypeName, c.consignerAccountNo, c.creditClientId, c.isApproved, 
		                        c.isPriceComputed, c.IsInvoiced, c.COD,
		                        isnull(c.pieces,'0') CNpieces, isnull(c.[weight],'0') CNWeight, 
		                        'BAG OUTPIECE' OPS, isnull(ml.pieces,'0') Pieces, 
		                        isnull(ml.[WEIGHT],'0') WEIGHT, 
		                        ml.bagNumber number, BB.name LOCATION 
		                        FROM #WEIGHT_DISCREPANCY__2618 C 
		                        INNER JOIN BagOutpieceAssociation ml ON c.consignmentNumber = ml.outpieceNumber  
		                        INNER JOIN BAG BG ON ML.bagNumber = BG.bagNumber 
		                        INNER JOIN Branches BB ON BG.branchCode = BB.branchCode 
		                        INNER JOIN Branches b1 ON c.orgin = b1.branchCode 
		                        INNER JOIN Branches b2 ON c.destination = b2.branchCode 
		                        INNER JOIN Zones z ON b1.zoneCode = z.zoneCode 
		                        INNER JOIN Zones z1 ON b2.zoneCode = z1.zoneCode 
		                        WHERE (convert(float,ml.[WEIGHT]) - c.[weight]) >= '{model.Weight}'
                               { (model.BookingCode != null || model.BookingCode != "" ? " " : $"AND C.riderCode = '{model.BookingCode}' ")} 
		                        AND Z.ZONECODE = '{u.ZoneCode}' AND b1.branchCode = '{u.BranchCode}'
	                        ) b
	                        INNER JOIN CreditClients cc ON b.creditClientId = cc.id 
	                        GROUP BY b.bookingrider, 
	                        b.consignmentNumber, CONVERT(VARCHAR(11),b.bookingDate,106),  
	                        orignZone,orignBranch, destinationBranch,destinationZone , b.serviceTypeName,  
	                        cc.name, cc.accountNo, b.CNpieces, b.CNWeight, 
	                        b.Pieces, b.[WEIGHT], b.Number, b.ops,B.LOCATION, 
	                        CASE WHEN isnull(b.isApproved,'0') = '1' THEN 'YES' ELSE 'NO' END, 
	                        CASE WHEN isnull(b.isPriceComputed,'0') = '1' THEN 'YES' ELSE 'NO' END, 
	                        CASE WHEN isnull(b.IsInvoiced,'0') = '1' THEN 'YES' ELSE 'NO' END, 
	                        CASE WHEN isnull(b.COD,'0') = '1' THEN 'COD' ELSE '' END 
	                        ORDER BY b.consignmentNumber, b.ops 
                        DROP TABLE #WEIGHT_DISCREPANCY__2618 ";

                var rs = await con.QueryAsync<WeightDisplayModel>(query);
                con.Close();
                return rs.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region Weight Detail       
        public async Task<WeightDisplayModel> GetWeightDetail(string CN)
        {
            try
            {
                var query = $@"SELECT c.* INTO #WEIGHT_DISCREPANCY__2618 FROM Consignment c 
                        INNER JOIN CreditClients cc ON cc.id = c.creditClientId AND cc.IndustryId != '67' 
                        WHERE c.consignmentNumber = '{CN}' and isnull(c.cod,'0') in ('0','1') 

	                        SELECT DISTINCT b.bookingrider,
	                        b.consignmentNumber, CONVERT(VARCHAR(11),b.bookingDate,106) BOOKINGDATE,  
	                        orignZone,orignBranch, destinationBranch,destinationZone , b.serviceTypeName,  
	                        cc.name clientname, cc.accountNo,  
	                        b.CNpieces, b.Pieces Ops_Pieces,
	                        b.CNWeight, b.[WEIGHT] Ops_weight,
	                        b.[WEIGHT] - b.CNWeight Weight_diff, 
	                        b.ops, b.Number Ops_Number, B.LOCATION, 
	                        CASE WHEN isnull(b.isApproved,'0') = '1' THEN 'YES' ELSE 'NO' END isApproved, 
	                        CASE WHEN isnull(b.isPriceComputed,'0') = '1' THEN 'YES' ELSE 'NO' END isPriceComputed, 
	                        CASE WHEN isnull(b.IsInvoiced,'0') = '1' THEN 'YES' ELSE 'NO' END IsInvoiced, 
	                        CASE WHEN isnull(b.COD,'0') = '1' THEN 'COD' ELSE '' END CODStatus 
                        FROM (
		                        SELECT  c.riderCode bookingrider,
		                        c.consignmentNumber, CONVERT(VARCHAR(11),c.bookingDate,106) BOOKINGDATE,  
		                        z.name orignZone,b1.name orignBranch, b2.name destinationBranch,z1.name destinationZone , c.serviceTypeName,  
		                        c.consignerAccountNo, c.creditClientId, c.isApproved, 
		                        c.isPriceComputed, c.IsInvoiced, c.COD, 
		                        isnull(c.pieces,'0') CNpieces, isnull(c.[weight],'0') CNWeight, 
		                        'MANIFEST' OPS, isnull(mc.Pieces,'0') Pieces, 
		                        cast(isnull(mc.[Weight],'0') AS VARCHAR) WEIGHT, 
		                        mc.manifestNumber Number, BB.name LOCATION 
		                        FROM #WEIGHT_DISCREPANCY__2618 C 
		                        INNER JOIN Mnp_ConsignmentManifest mc ON c.consignmentNumber = mc.consignmentNumber 
		                        INNER JOIN Mnp_Manifest mm ON MC.manifestNumber = MM.manifestNumber 
		                        INNER JOIN Branches BB ON MM.branchCode = BB.branchCode 
		                        INNER JOIN Branches b1 ON c.orgin = b1.branchCode 
		                        INNER JOIN Branches b2 ON c.destination = b2.branchCode 
		                        INNER JOIN Zones z ON b1.zoneCode = z.zoneCode 
		                        INNER JOIN Zones z1 ON b2.zoneCode = z1.zoneCode 
		                        WHERE C.consignmentNumber = '{CN}'
                               
	                        UNION ALL  
		                        SELECT c.riderCode bookingrider, 
		                        c.consignmentNumber, CONVERT(VARCHAR(11),c.bookingDate,106) BOOKINGDATE,  
		                        z.name orignZone,b1.name orignBranch, b2.name destinationBranch,z1.name destinationZone , c.serviceTypeName,   
		                        c.consignerAccountNo, c.creditClientId, c.isApproved, 
		                        c.isPriceComputed, c.IsInvoiced, c.COD,
		                        isnull(c.pieces,'0') CNpieces, isnull(c.[weight],'0') CNWeight, 
		                        'LOADING' OPS, isnull(ml.cnPieces,'0') Pieces, 
		                        isnull(ml.CNWeight,'0') WEIGHT, Cast(isnull(ml.loadingId,'0') AS VARCHAR) number, 
		                        BB.name LOCATION
		                        FROM #WEIGHT_DISCREPANCY__2618 C 
		                        INNER JOIN MnP_LoadingConsignment ml ON c.consignmentNumber = ml.consignmentNumber  
		                        INNER JOIN MnP_Loading L ON ML.loadingId = L.id 
		                        INNER JOIN Branches bB ON L.branchCode = BB.branchCode 
		                        INNER JOIN Branches b1 ON c.orgin = b1.branchCode 
		                        INNER JOIN Branches b2 ON c.destination = b2.branchCode 
		                        INNER JOIN Zones z ON b1.zoneCode = z.zoneCode 
		                        INNER JOIN Zones z1 ON b2.zoneCode = z1.zoneCode 
		                        WHERE C.consignmentNumber = '{CN}'
	                        UNION ALL  
		                        SELECT c.riderCode bookingrider, 
		                        c.consignmentNumber, CONVERT(VARCHAR(11),c.bookingDate,106) BOOKINGDATE,  
		                        z.name orignZone,b1.name orignBranch, b2.name destinationBranch,z1.name destinationZone , 
		                        c.serviceTypeName, c.consignerAccountNo, c.creditClientId, c.isApproved, 
		                        c.isPriceComputed, c.IsInvoiced, c.COD,
		                        isnull(c.pieces,'0') CNpieces, isnull(c.[weight],'0') CNWeight, 
		                        'BAG OUTPIECE' OPS, isnull(ml.pieces,'0') Pieces, 
		                        isnull(ml.[WEIGHT],'0') WEIGHT, 
		                        ml.bagNumber number, BB.name LOCATION 
		                        FROM #WEIGHT_DISCREPANCY__2618 C 
		                        INNER JOIN BagOutpieceAssociation ml ON c.consignmentNumber = ml.outpieceNumber  
		                        INNER JOIN BAG BG ON ML.bagNumber = BG.bagNumber 
		                        INNER JOIN Branches BB ON BG.branchCode = BB.branchCode 
		                        INNER JOIN Branches b1 ON c.orgin = b1.branchCode 
		                        INNER JOIN Branches b2 ON c.destination = b2.branchCode 
		                        INNER JOIN Zones z ON b1.zoneCode = z.zoneCode 
		                        INNER JOIN Zones z1 ON b2.zoneCode = z1.zoneCode 
		                        WHERE C.consignmentNumber = '{CN}'
	                        ) b
	                        INNER JOIN CreditClients cc ON b.creditClientId = cc.id 
	                        GROUP BY b.bookingrider, 
	                        b.consignmentNumber, CONVERT(VARCHAR(11),b.bookingDate,106),  
	                        orignZone,orignBranch, destinationBranch,destinationZone , b.serviceTypeName,  
	                        cc.name, cc.accountNo, b.CNpieces, b.CNWeight, 
	                        b.Pieces, b.[WEIGHT], b.Number, b.ops,B.LOCATION, 
	                        CASE WHEN isnull(b.isApproved,'0') = '1' THEN 'YES' ELSE 'NO' END, 
	                        CASE WHEN isnull(b.isPriceComputed,'0') = '1' THEN 'YES' ELSE 'NO' END, 
	                        CASE WHEN isnull(b.IsInvoiced,'0') = '1' THEN 'YES' ELSE 'NO' END, 
	                        CASE WHEN isnull(b.COD,'0') = '1' THEN 'COD' ELSE '' END 
	                        ORDER BY b.consignmentNumber, b.ops 
                        DROP TABLE #WEIGHT_DISCREPANCY__2618 ";

                var rs = await con.QueryFirstOrDefaultAsync<WeightDisplayModel>(query);
                con.Close();
                return rs;
            }
            catch (Exception ex)
            {
				con.Close();
				throw ex;
            }

        }

		public async Task<int> GetWeightUpdated(string CN)
		{
			try
			{
				var query = $@"select count(*) from Mnp_WeightConfirm where CN='{CN}' ";

				var rs = await con.QueryFirstOrDefaultAsync<int>(query);
				con.Close();
				return rs;
			}
			catch (Exception ex)
			{
				con.Close();
				throw ex;
			}

		}
		public async Task<dynamic> AddRemarks(string CN, UserModel u, string Remarks, int IsCorrect)
        {
            SqlTransaction trans = null;
            long inscheck = 0;
            try
            {
                await con.OpenAsync();
                trans = con.BeginTransaction();

                var query = $@"Insert into Mnp_WeightConfirm values
                               (@CN, @WeightCorrect, @UserId, @Today, @Remarks, @Branch, @Zone) ;";
                inscheck = await con.ExecuteAsync(query, new { CN = CN, WeightCorrect = IsCorrect, UserId = u.Uid, Today=DateTime.Now, Remarks = Remarks, Branch = u.BranchCode, Zone = u.ZoneCode }, transaction: trans);

                trans.Commit();
                con.Close();
                return inscheck;
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
        #endregion
    }
}