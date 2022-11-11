using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHST.Models;
using NHST.Bussiness;
using WebUI.Business;
using System.Data;
using MB.Extensions;

namespace NHST.Controllers
{
    public class WorkingSessionController
    {
        public static string Insert(string NameSession, double Weight, double Volume, int Status, DateTime CreatedDate, string CreatedBy, string NoteSession)
        {
            using (var dbe = new NHSTEntities())
            {
                dbe.Configuration.ValidateOnSaveEnabled = false;
                tbl_WorkingSession a = new tbl_WorkingSession();
                a.NameSession = NameSession;
                a.Weight = Weight;
                a.Volume = Volume;
                a.Status = Status;
                a.CreatedDate = CreatedDate;
                a.CreatedBy = CreatedBy;
                a.NoteSession = NoteSession;
                dbe.tbl_WorkingSession.Add(a);
                int kq = dbe.SaveChanges();
                string k = a.ID.ToString();
                return k;
            }
        }

        public static List<tbl_WorkingSession> GetAllByStatus(int Status)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_WorkingSession> las = new List<tbl_WorkingSession>();
                las = dbe.tbl_WorkingSession.Where(a => a.Status == Status).ToList();
                if (las.Count > 0)
                {
                    return las;
                }
                else return las;
            }
        }
        public static tbl_WorkingSession GetByNameSession(string NameSession)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_WorkingSession a = dbe.tbl_WorkingSession.Where(ad => ad.NameSession == NameSession).FirstOrDefault();
                if (a != null)
                {
                    return a;
                }
                else
                {
                    return null;
                }

            }
        }
        public static tbl_WorkingSession GetByID(int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_WorkingSession a = dbe.tbl_WorkingSession.Where(ad => ad.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    return a;
                }
                else
                {
                    return null;
                }

            }
        }

        public static string UpdateStatus(int ID, int Status, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_WorkingSession.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.Status = Status;
                    a.ModifiedBy = ModifiedBy;
                    a.ModifiedDate = ModifiedDate;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateNote(int ID, string NoteSession, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_WorkingSession.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.NoteSession = NoteSession;
                    a.ModifiedBy = ModifiedBy;
                    a.ModifiedDate = ModifiedDate;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateWeight(int ID, double Weight, int TotalPackage)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_WorkingSession.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.Weight = Weight;
                    a.TotalPackage = TotalPackage;                   
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static int GetTotal(string s, int stt)
        {
            var sql = @"select Total=Count(*) ";
            sql += "From tbl_WorkingSession ";
            sql += "Where NameSession Like N'%" + s + "%' ";
            if (stt > 0)
            {
                sql += " AND Status =" + stt + "";
            }
            int a = 0;
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }

        public static List<tbl_WorkingSession> GetAllPageSize(string s, int stt, int pageIndex, int pageSize)
        {
            var sql = @"select * ";
            sql += "From tbl_WorkingSession ";
            sql += "Where NameSession Like N'%" + s + "%' ";
            if (stt > 0)
            {
                sql += " AND Status =" + stt + "";
            }
            sql += "order by ID DESC OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            List<tbl_WorkingSession> a = new List<tbl_WorkingSession>();
            while (reader.Read())
            {
                var entity = new tbl_WorkingSession();

                int ID = 0;
                if (reader["ID"] != DBNull.Value)
                {
                    entity.ID = reader["ID"].ToString().ToInt(0);
                    ID = Convert.ToInt32(reader["ID"].ToString());
                }

                if (reader["NameSession"] != DBNull.Value)
                    entity.NameSession = reader["NameSession"].ToString();

                if (reader["NoteSession"] != DBNull.Value)
                    entity.NoteSession = reader["NoteSession"].ToString();

                if (reader["CreatedBy"] != DBNull.Value)
                    entity.CreatedBy = reader["CreatedBy"].ToString();

                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());

                double Weight = 0;
                int TotalPackage = 0;
                if (ID > 0)
                {
                    var sm = SmallPackageController.GetBySessionID(ID);
                    if (sm != null)
                    {
                        if (sm.Count > 0)
                        {
                            TotalPackage = sm.Count;
                            double totalweight = 0;
                            foreach (var item in sm)
                            {
                                double compareSize = 0;
                                double weight = Convert.ToDouble(item.Weight);
                                double pDai = Convert.ToDouble(item.Length);
                                double pRong = Convert.ToDouble(item.Width);
                                double pCao = Convert.ToDouble(item.Height);
                                if (pDai > 0 && pRong > 0 && pCao > 0)
                                {
                                    compareSize = (pDai * pRong * pCao) / 6000;
                                }
                                if (weight >= compareSize)
                                {
                                    totalweight += weight;
                                }
                                else
                                {
                                    totalweight += compareSize;
                                }
                            }
                            Weight = totalweight;
                        }    
                    }
                }

                var session = WorkingSessionController.GetByID(ID);
                if (session != null)
                {
                    WorkingSessionController.UpdateWeight(ID, Weight, TotalPackage);
                }    

                entity.Weight = Weight;
                entity.TotalPackage = TotalPackage;

                a.Add(entity);
            }
            reader.Close();
            return a;
        }

        public class WorkingSession
        {
            public int ID { get; set; }
            public string NameSession { get; set; }
            public double Weight { get; set; }
            public double Volume { get; set; }
            public int Status { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime ModifiedDate { get; set; }
            public int TotalPackage { get; set; }
        }

    }
}