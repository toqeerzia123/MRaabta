using Dapper;
using MRaabta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MRaabta.Repo.Api
{
    public class PRARepo
    {
        SqlConnection con;
        public PRARepo()
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
        public async Task<List<dynamic>> GetData()
        {
            try
            {
                var query = $@"select
                                pc.ConsignmentNumber as CN,
                                pc.AccountReceivingDate,
                                pc.BuyerCNIC,
                                pc.BuyerName,
                                pc.BuyerPhoneNumber,
                                cast(pc.TotalBillAmount as decimal(12,2)) as TotalBillAmount,
                                cast(pc.TotalQuantity as decimal(12,2)) as TotalQuantity,
                                cast(pc.TotalSaleValue as decimal(12,2)) as TotalSaleValue,
                                cast(pc.TotalTaxCharged as decimal(12,2)) as TotalTaxCharged,
                                pc.Service,
                                pc.TaxRate
                                from PRAConsignments pc
                                where (cast(pc.AccountReceivingDate as date) = cast(getdate() - 3 as date) or pc.InvoiceDate is not null) and InvoiceNumber is null;";
                var rs = await con.QueryAsync(query, commandTimeout: int.MaxValue);
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
        public async Task<bool> UpdateCN(string cn, string invno, string code, string response)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "PRALog.txt";
            var query = "";
            try
            {
                query = $@"update PRAConsignments set InvoiceDate = getdate(), InvoiceNumber = {(string.IsNullOrEmpty(invno) ? "null" : $"'{invno}'")}, Code = {(string.IsNullOrEmpty(code) ? "null" : code)}, Response = '{response}' where ConsignmentNumber = '{cn}';";
                var rs = await con.ExecuteAsync(query);
                return rs > 0;
            }
            catch (SqlException ex)
            {
                File.AppendAllText(path, query + "\n");
                return false;
            }
            catch (Exception ex)
            {
                File.AppendAllText(path, query + "\n");
                return false;
            }
        }
        public async Task<HttpResponseMessage> PRACall(string url, string json)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                //For Sandbox
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "2feedfc3-04fd-369c-b7bb-9026bf2ae7ec");
                //For Production
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "92e46e9e-9a7b-338f-aafc-fdefef3778e4");
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                return await client.PostAsync(url, content);
            }
        }
        public async Task<List<DropDownModel>> Zones()
        {
            try
            {
                await con.OpenAsync();
                var rs = await con.QueryAsync<DropDownModel>(@"select distinct z.zoneCode as Value, z.name as Text
                                                                from Zones z
                                                                inner join Branches b on b.zoneCode = z.zoneCode and b.[status] = 1
                                                                where z.status = 1 and zone_type = 1 and b.branchCode in(
                                                                61,
                                                                60,
                                                                12,
                                                                13,
                                                                136,
                                                                40,
                                                                80,
                                                                82,
                                                                86,
                                                                135,
                                                                100,
                                                                105,
                                                                11,
                                                                14,
                                                                38,
                                                                153,
                                                                154,
                                                                141,
                                                                170,
                                                                34,
                                                                37,
                                                                48,
                                                                188,
                                                                55,
                                                                8,
                                                                66,
                                                                73,
                                                                7,
                                                                72,
                                                                75,
                                                                6,
                                                                254,
                                                                108,
                                                                120,
                                                                9,
                                                                116,
                                                                130,
                                                                50,
                                                                83,
                                                                1,
                                                                90,
                                                                172,
                                                                107,
                                                                121,
                                                                143,
                                                                44,
                                                                49,
                                                                20,
                                                                139,
                                                                53,
                                                                59,
                                                                21,
                                                                291,
                                                                70,
                                                                290,
                                                                144,
                                                                81,
                                                                85,
                                                                236,
                                                                145,
                                                                146,
                                                                140,
                                                                243,
                                                                147,
                                                                89,
                                                                18,
                                                                97,
                                                                3,
                                                                142,
                                                                19,
                                                                148,
                                                                137,
                                                                2,
                                                                149,
                                                                138
                                                                ) order by z.name;");
                con.Close();
                return rs.ToList();
            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
                throw ex;
            }
        }
        public async Task<List<dynamic>> DifferenceInChargeAmountReport(string zoneCode, DateTime dt)
        {
            try
            {
                var query = $@"select
                            z.Region as Region,
                            z.name as Zone,
                            b.sname as Branch,
                            format(c.accountReceivingDate,'dd-MMM-yyyy') as AccoutReceivingDate,
                            c.consignmentNumber as CN, 
                            c.consigner as Consigner,
                            c.consignerCNICNo as ConsignerNTN,
                            zz.name as DestZone,
                            bb.sname as DestBranch, 
                            c.serviceTypeName as Service, 
                            c.weight as Weight,
                            c.pieces as Pcs, 
                            c.chargedAmount as CNChargedAmount,
                            pc.TotalBillAmount PRAChargedAmount,
                            format(pc.InvoiceDate,'dd-MMM-yyyy hh:mm tt') as InvoiceDate,
                            pc.InvoiceNumber,
                            pc.InvoiceType 
                            from consignment c 
                            inner join PRAConsignments pc on pc.ConsignmentNumber = c.consignmentNumber
                            inner join Branches b on c.orgin = b.branchCode
                            inner join zones z on b.zoneCode = z.zoneCode
                            inner join Branches bb on c.destination = bb.branchCode
                            inner join zones zz on bb.zoneCode = zz.zoneCode
                            inner join CreditClients cc on c.creditClientId = cc.id
                            where year(c.AccountReceivingDate) = {dt.Year}
                            and month(c.AccountReceivingDate) = {dt.Month}
                            and c.consignerAccountNo = '0'
                            and isnull(c.isapproved,'0') = '1'
                            and zz.zoneCode = '{zoneCode}'
                            and c.branchCode in(
                            61,
                            60,
                            12,
                            13,
                            136,
                            40,
                            80,
                            82,
                            86,
                            135,
                            100,
                            105,
                            11,
                            14,
                            38,
                            153,
                            154,
                            141,
                            170,
                            34,
                            37,
                            48,
                            188,
                            55,
                            8,
                            66,
                            73,
                            7,
                            72,
                            75,
                            6,
                            254,
                            108,
                            120,
                            9,
                            116,
                            130,
                            50,
                            83,
                            1,
                            90,
                            172,
                            107,
                            121,
                            143,
                            44,
                            49,
                            20,
                            139,
                            53,
                            59,
                            21,
                            291,
                            70,
                            290,
                            144,
                            81,
                            85,
                            236,
                            145,
                            146,
                            140,
                            243,
                            147,
                            89,
                            18,
                            97,
                            3,
                            142,
                            19,
                            148,
                            137,
                            2,
                            149,
                            138
                            )                            
                            and pc.TotalBillAmount <> c.chargedAmount
                            and pc.InvoiceType = 'i';";
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
        public async Task<List<dynamic>> TodayDataToBeShared()
        {
            try
            {
                var query = $@"select
                            z.Region as Region,
                            z.name as Zone,
                            b.sname as Branch,
                            format(c.accountReceivingDate,'dd-MMM-yyyy') as AccoutReceivingDate,
                            c.consignmentNumber as CN, 
                            c.consigner as Consigner,
                            c.consignerCNICNo as ConsignerNTN,
                            zz.name as DestZone,
                            bb.sname as DestBranch, 
                            c.serviceTypeName as Service, 
                            c.weight as Weight,
                            c.pieces as Pcs, 
                            c.chargedAmount as CNChargedAmount,
                            pc.TotalBillAmount PRAChargedAmount,
                            format(pc.InvoiceDate,'dd-MMM-yyyy hh:mm tt') as InvoiceDate,
                            pc.InvoiceNumber,
                            pc.InvoiceType 
                            From consignment c
                            inner join PRAConsignments pc on pc.ConsignmentNumber = c.consignmentNumber
                            inner join Branches b on c.orgin = b.branchCode
                            inner join zones z on b.zoneCode = z.zoneCode
                            inner join Branches bb on c.destination = bb.branchCode
                            inner join zones zz on bb.zoneCode = zz.zoneCode
                            inner join CreditClients cc on c.creditClientId = cc.id
                            where
                            cast(c.AccountReceivingDate as date) = cast(getdate() - 3 as date)
                            and pc.InvoiceNumber is null
                            and pc.InvoiceType = 'i';";
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
        public async Task<List<dynamic>> RemainingData()
        {
            try
            {
                var query = $@"select
                                z.Region as Region,
                                z.name as Zone,
                                b.sname as Branch,
                                format(c.accountReceivingDate,'dd-MMM-yyyy') as AccoutReceivingDate,
                                c.consignmentNumber as CN, 
                                c.consigner as Consigner,
                                c.consignerCNICNo as ConsignerNTN,
                                zz.name as DestZone,
                                bb.sname as DestBranch, 
                                c.serviceTypeName as Service, 
                                c.weight as Weight,
                                c.pieces as Pcs, 
                                c.chargedAmount as CNChargedAmount,
                                pc.TotalBillAmount PRAChargedAmount,
                                format(pc.InvoiceDate,'dd-MMM-yyyy') as InvoiceDate,
                                pc.InvoiceNumber,
                                pc.InvoiceType 
                                From consignment c
                                left join PRAConsignments pc on c.consignmentNumber = pc.ConsignmentNumber
                                inner join Branches b on c.orgin = b.branchCode
                                inner join zones z on b.zoneCode = z.zoneCode
                                inner join Branches bb on c.destination = bb.branchCode
                                inner join zones zz on bb.zoneCode = zz.zoneCode
                                inner join CreditClients cc on c.creditClientId = cc.id
                                where 
                                year(c.AccountReceivingDate) = year(getdate())
                                and month(c.AccountReceivingDate) = month(getdate())
                                and cast(c.AccountReceivingDate as date) = cast(getdate() - 3 as date)
                                and c.branchCode in(
                                61,
                                60,
                                12,
                                13,
                                136,
                                40,
                                80,
                                82,
                                86,
                                135,
                                100,
                                105,
                                11,
                                14,
                                38,
                                153,
                                154,
                                141,
                                170,
                                34,
                                37,
                                48,
                                188,
                                55,
                                8,
                                66,
                                73,
                                7,
                                72,
                                75,
                                6,
                                254,
                                108,
                                120,
                                9,
                                116,
                                130,
                                50,
                                83,
                                1,
                                90,
                                172,
                                107,
                                121,
                                143,
                                44,
                                49,
                                20,
                                139,
                                53,
                                59,
                                21,
                                291,
                                70,
                                290,
                                144,
                                81,
                                85,
                                236,
                                145,
                                146,
                                140,
                                243,
                                147,
                                89,
                                18,
                                97,
                                3,
                                142,
                                19,
                                148,
                                137,
                                2,
                                149,
                                138
                                ) 
                                and c.consignerAccountNo = '0'
                                and isnull(c.isapproved,'0') <> '1'";
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
        public async Task<List<dynamic>> FailedToSendData()
        {
            try
            {
                var query = $@"select
                            z.Region as Region,
                            z.name as Zone,
                            b.sname as Branch,
                            format(c.accountReceivingDate,'dd-MMM-yyyy') as AccoutReceivingDate,
                            c.consignmentNumber as CN, 
                            c.consigner as Consigner,
                            c.consignerCNICNo as ConsignerNTN,
                            zz.name as DestZone,
                            bb.sname as DestBranch, 
                            c.serviceTypeName as Service, 
                            c.weight as Weight,
                            c.pieces as Pcs, 
                            c.chargedAmount as CNChargedAmount,
                            pc.TotalBillAmount PRAChargedAmount,
                            format(pc.InvoiceDate,'dd-MMM-yyyy hh:mm tt') as InvoiceDate,
                            pc.InvoiceNumber,
                            pc.InvoiceType 
                            from PRAConsignments pc
                            inner join consignment c on pc.ConsignmentNumber = c.consignmentNumber
                            inner join Branches b on c.orgin = b.branchCode
                            inner join zones z on b.zoneCode = z.zoneCode
                            inner join Branches bb on c.destination = bb.branchCode
                            inner join zones zz on bb.zoneCode = zz.zoneCode
                            inner join CreditClients cc on c.creditClientId = cc.id
                            where 
                            pc.InvoiceDate is not null and pc.InvoiceNumber is null
                            and pc.InvoiceType = 'i';";
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
        public async Task<List<dynamic>> QualifiedButNotSend()
        {
            try
            {
                var query = $@"select 
                                    z.Region as Region,
                                    z.name as Zone,
                                    b.sname as Branch,
                                    format(c.accountReceivingDate,'dd-MMM-yyyy') as AccoutReceivingDate,
                                    c.consignmentNumber as CN, 
                                    c.consigner as Consigner,
                                    c.consignerCNICNo as ConsignerNTN,
                                    zz.name as DestZone,
                                    bb.sname as DestBranch, 
                                    c.serviceTypeName as Service, 
                                    c.weight as Weight,
                                    c.pieces as Pcs, 
                                    c.chargedAmount as CNChargedAmount,
                                    pc.TotalBillAmount as PRAChargedAmount,
                                    format(pc.InvoiceDate,'dd-MMM-yyyy') as InvoiceDate,
                                    pc.InvoiceNumber,
                                    pc.InvoiceType 
                                    from PRAConsignments pc
                                    inner join consignment c on pc.ConsignmentNumber = c.consignmentNumber and cast(c.accountReceivingDate as date) = cast(pc.AccountReceivingDate as date)
                                    inner join Branches b on c.orgin = b.branchCode
                                    inner join zones z on b.zoneCode = z.zoneCode
                                    inner join Branches bb on c.destination = bb.branchCode
                                    inner join zones zz on bb.zoneCode = zz.zoneCode
                                    inner join CreditClients cc on c.creditClientId = cc.id
                                    where
                                    YEAR(c.accountReceivingDate) = year(getdate()) 
                                    and month(c.accountReceivingDate) = month(getdate())
                                    and CAST(c.accountreceivingdate AS DATE) <= CAST(GETDATE() - 3 AS DATE)
                                    and c.branchCode in(
                                    61,
                                    60,
                                    12,
                                    13,
                                    136,
                                    40,
                                    80,
                                    82,
                                    86,
                                    135,
                                    100,
                                    105,
                                    11,
                                    14,
                                    38,
                                    153,
                                    154,
                                    141,
                                    170,
                                    34,
                                    37,
                                    48,
                                    188,
                                    55,
                                    8,
                                    66,
                                    73,
                                    7,
                                    72,
                                    75,
                                    6,
                                    254,
                                    108,
                                    120,
                                    9,
                                    116,
                                    130,
                                    50,
                                    83,
                                    1,
                                    90,
                                    172,
                                    107,
                                    121,
                                    143,
                                    44,
                                    49,
                                    20,
                                    139,
                                    53,
                                    59,
                                    21,
                                    291,
                                    70,
                                    290,
                                    144,
                                    81,
                                    85,
                                    236,
                                    145,
                                    146,
                                    140,
                                    243,
                                    147,
                                    89,
                                    18,
                                    97,
                                    3,
                                    142,
                                    19,
                                    148,
                                    137,
                                    2,
                                    149,
                                    138
                                    )
                                    and c.consignerAccountNo = '0'
                                    and isnull(c.isapproved,'0') = '1'
                                    and pc.InvoiceNumber is null;";
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
        public async Task<List<dynamic>> SummaryReport(int year, int month)
        {
            try
            {
                var query = $@"with t as (
                                        select 
                                        z.Region as Region,
                                        z.name as Zone,
                                        b.sname as Branch,
                                        case when pc.InvoiceNumber is not null then format(pc.AccountReceivingDate,'dd-MMM-yyyy') else format(c.accountReceivingDate,'dd-MMM-yyyy') end as AccoutReceivingDate,
                                        pc.InvoiceType,
                                        c.consignmentNumber as BookedCN,
                                        case when pc.InvoiceNumber is not null then 1 else 0 end as InvoicedCN
                                        from consignment c
                                        left join PRAConsignments pc on pc.ConsignmentNumber = c.consignmentNumber
                                        inner join Branches b on c.orgin = b.branchCode
                                        inner join zones z on b.zoneCode = z.zoneCode
                                        inner join CreditClients cc on c.creditClientId = cc.id
                                        where
                                        YEAR(c.accountReceivingDate) = {year}
                                        and month(c.accountReceivingDate) = {month}
                                        and c.branchCode in(
                                        61,
                                        60,
                                        12,
                                        13,
                                        136,
                                        40,
                                        80,
                                        82,
                                        86,
                                        135,
                                        100,
                                        105,
                                        11,
                                        14,
                                        38,
                                        153,
                                        154,
                                        141,
                                        170,
                                        34,
                                        37,
                                        48,
                                        188,
                                        55,
                                        8,
                                        66,
                                        73,
                                        7,
                                        72,
                                        75,
                                        6,
                                        254,
                                        108,
                                        120,
                                        9,
                                        116,
                                        130,
                                        50,
                                        83,
                                        1,
                                        90,
                                        172,
                                        107,
                                        121,
                                        143,
                                        44,
                                        49,
                                        20,
                                        139,
                                        53,
                                        59,
                                        21,
                                        291,
                                        70,
                                        290,
                                        144,
                                        81,
                                        85,
                                        236,
                                        145,
                                        146,
                                        140,
                                        243,
                                        147,
                                        89,
                                        18,
                                        97,
                                        3,
                                        142,
                                        19,
                                        148,
                                        137,
                                        2,
                                        149,
                                        138
                                        )
                                        and c.consignerAccountNo = '0'
                                        )
                                        select 
                                        t.Region,
                                        t.[Zone],
                                        t.Branch,
                                        t.AccoutReceivingDate,
                                        t.InvoiceType,
                                        count(t.BookedCN) as TotalBookedCNs,
                                        sum(t.InvoicedCN) as InvoicedCns
                                        from t
                                        GROUP by 
                                        t.Region,
                                        t.[Zone],
                                        t.Branch,
                                        t.AccoutReceivingDate,
                                        t.InvoiceType;";
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
    }
}