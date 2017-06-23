using importAccountExcel.Data;
using importAccountExcel.TallyDb;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace importAccountExcel
{
    class Program
    {
        static void Main(string[] args)
        {
            ExceldataEntities database = new ExceldataEntities();
            Excel.Application xlapp = new Excel.Application();
            Excel.Workbook xlworkbook = xlapp.Workbooks.Open(@"C:\Users\Administrator\Desktop\backup\NALLAYARASUS.xls");
            Excel.Worksheet xlworksheet = xlworkbook.Sheets[1];
            Excel.Range xlrange = xlworksheet.UsedRange;
            int Lid = 1;
            for (int i = 10; i <= 3351; i++)
            {
                for (int j = 1; j <= xlrange.Columns.Count; j++)
                {

                    //Console.Write(string.Format("cells[{0},{1}] ", i, j));
                    if (j == 1)
                    {
                        if (xlrange.Cells[i, j].Value2 != null)
                        {
                            if (!string.IsNullOrEmpty(xlrange.Cells[i, j].Value2.ToString()))
                            {
                                double dt = double.Parse(xlrange.Cells[i, j].Value2.ToString());
                                DateTime time = DateTime.FromOADate(dt);
                                // Console.WriteLine("found new master record...");
                                string balance = xlrange.Cells[i, 13].Value2.ToString();
                                // balance=balance.Substring(balance.Length - 3);
                                MasterTable mt = new MasterTable()
                                {
                                    dt = Convert.ToDateTime(time),
                                    toby = xlrange.Cells[i, 2].Value2.ToString(),
                                    particular = xlrange.Cells[i, 3].Value2.ToString(),
                                    voucherType = xlrange.Cells[i, 9].Value2.ToString(),
                                    vchno = Convert.ToInt16(xlrange.Cells[i, 10].Value2.ToString()),
                                    debit = Convert.ToDecimal(xlrange.Cells[i, 11].Value2),
                                    credit = Convert.ToDecimal(xlrange.Cells[i, 12].Value2),
                                    balance = Convert.ToDecimal(balance),
                                    AccountDetail = xlrange.Cells[4, 1].Value2.ToString()
                                };
                                database.MasterTable.Add(mt);
                                database.SaveChanges();
                                Lid = database.MasterTable.Max(m => m.id);
                            }
                            else
                            {
                                DateTime? time2 = null;
                                decimal? amt = null;
                                double? dateval = xlrange.Cells[i, 5].Value2;
                                var stramt = xlrange.Cells[i, 6].Value2;
                                if (stramt != null)
                                {
                                    amt = Convert.ToDecimal(stramt);
                                }
                                if (dateval != null)
                                {
                                    double dt2 = double.Parse(xlrange.Cells[i, 5].Value2.ToString());
                                    time2 = DateTime.FromOADate(dt2);
                                }

                                secMstr secmaster = new secMstr()
                                {
                                    head = Convert.ToString(xlrange.Cells[i, 3].Value2.ToString()),
                                    amtcheq = Convert.ToString(xlrange.Cells[i, 4].Value2.ToString()),
                                    dt = time2,
                                    amount = amt,
                                    mstrid = Lid,
                                };
                                database.secMstr.Add(secmaster);
                                database.SaveChanges();

                            }

                        }
                    }

                    //if (xlrange.Cells[i, j].Value2 != null)
                    //{

                    //   // Console.Write(xlrange.Cells[i, j] == null ? " " : xlrange.Cells[i, j].Value2.ToString());

                    //}
                    //Console.WriteLine();
                }
                int percentage = (i * 100) / 3351;
                Console.Write("\r{0}%   ", percentage);
            }
        }
    }


    public class normalising{
        public static ICSRDBTALLYEntities icsrdbtally = new ICSRDBTALLYEntities();
        SqlConnection con = new SqlConnection("");
        public void chenageIt()
        {
            SqlCommand sqlcmd = new SqlCommand(@"select * from [ICSRDBTALLY].[dbo].[Vouchers] where LedgerName not like 'NALLAYARASU A/c 2722101012096' and ISNULL(void,'N')='N' and TallyMasterid in(select TallyMasterid from [ICSRDBTALLY].[dbo].[Vouchers] where LedgerName like 'NALLAYARASU A/c 2722101012096' and ISNULL(void,'N')='N') order by VoucherDate asc", new SqlConnection(@"Data Source = USER1 - PC; Initial Catalog = FACCT; Integrated Security = False; User Id = sa; Password = IcsR@123#;MultipleActiveResultSets=True"));
          SqlDataReader dr=  sqlcmd.ExecuteReader();
            while (dr.Read())
            {
                var 
            }
        }
    }
}
