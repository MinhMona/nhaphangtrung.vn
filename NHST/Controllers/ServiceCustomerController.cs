using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHST.Models;

namespace NHST.Controllers
{
    public class ServiceCustomerController
    {
        #region 
        public static tbl_ServiceCustomer Insert(string ServiceOfName, string ServiceOfContent, string ServiceOfLink, string ServiceOfIMG, bool IsHidden, int Position, string Created)
        {
            using (var db = new NHSTEntities())
            {
                tbl_ServiceCustomer sv = new tbl_ServiceCustomer();
                sv.ServiceOfName = ServiceOfName;
                sv.ServiceOfContent = ServiceOfContent;
                sv.ServiceOfLink = ServiceOfLink;
                sv.ServiceOfIMG = ServiceOfIMG;
                sv.IsHidden = IsHidden;
                sv.Position = Position;
                sv.CreatedBy = Created;
                sv.CreatedDate = DateTime.Now;
                db.tbl_ServiceCustomer.Add(sv);
                db.SaveChanges();
                return sv;
            }
        }

        public static tbl_ServiceCustomer Update(int ID, string ServiceOfName, string ServiceOfContent, string ServiceOfLink, string ServiceOfIMG, bool IsHidden, int Position, string Created)
        {
            using (var db = new NHSTEntities())
            {
                var sv = db.tbl_ServiceCustomer.Where(x => x.ID == ID).FirstOrDefault();
                if (sv != null)
                {
                    sv.ServiceOfName = ServiceOfName;
                    sv.ServiceOfContent = ServiceOfContent;
                    sv.ServiceOfLink = ServiceOfLink;
                    if (!string.IsNullOrEmpty(ServiceOfIMG))
                        sv.ServiceOfIMG = ServiceOfIMG;
                    sv.IsHidden = IsHidden;
                    sv.Position = Position;
                    sv.ModifiedBy = Created;
                    sv.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    return sv;
                }
                return null;
            }
        }
        #endregion


        #region Select
        public static List<tbl_ServiceCustomer> GetAllAD()
        {
            using (var db = new NHSTEntities())
            {
                var sv = db.tbl_ServiceCustomer.ToList();
                if (sv.Count > 0)
                    return sv;
                return null;
            }
        }

        public static List<tbl_ServiceCustomer> GetAll()
        {
            using (var db = new NHSTEntities())
            {
                var sv = db.tbl_ServiceCustomer.Where(x => x.IsHidden != true).ToList();
                if (sv.Count > 0)
                    return sv;
                return null;
            }
        }

        public static tbl_ServiceCustomer GetByID(int ID)
        {
            using (var db = new NHSTEntities())
            {
                var sv = db.tbl_ServiceCustomer.Where(x => x.ID == ID).FirstOrDefault();
                if (sv != null)
                    return sv;
                return null;
            }
        }
        #endregion
    }
}