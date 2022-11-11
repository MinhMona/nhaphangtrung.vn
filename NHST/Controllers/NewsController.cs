using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHST.Controllers
{
    public class NewsController
    {
        public static tbl_News GetByID(int ID)
        {
            using (var db = new NHSTEntities())
            {
                var sv = db.tbl_News.Where(x => x.ID == ID).FirstOrDefault();
                if (sv != null)
                    return sv;
                return null;
            }
        }
        public static tbl_News Update(int ID, string NewsTitle, string NewsLink, bool IsHidden, string NewsDesc, string NewsIMG, int NewsPosition, string Created, int Type)
        {
            using (var db = new NHSTEntities())
            {
                var sv = db.tbl_News.Where(x => x.ID == ID).FirstOrDefault();
                if (sv != null)
                {
                    sv.NewsTitle = NewsTitle;
                    sv.NewsLink = NewsLink;
                    sv.IsHidden = IsHidden;
                    if (!string.IsNullOrEmpty(NewsIMG))
                        sv.NewsIMG = NewsIMG;
                    sv.NewsDesc = NewsDesc;
                    sv.NewsPosition = NewsPosition;
                    sv.ModifiedBy = Created;
                    sv.ModifiedDate = DateTime.Now;
                    sv.Type = Type;
                    db.SaveChanges();
                    return sv;
                }
                return null;
            }
        }
        public static List<tbl_News> GetAll()
        {
            using (var db = new NHSTEntities())
            {
                List<tbl_News> ctb = new List<tbl_News>();
                ctb = db.tbl_News.OrderBy(x => x.NewsPosition).OrderBy(x => x.Type).ToList();
                return ctb;
            }
        }
        public static List<tbl_News> GetAllNotHiddenType(int Type)
        {
            using (var db = new NHSTEntities())
            {
                List<tbl_News> ctb = new List<tbl_News>();
                ctb = db.tbl_News.Where(x => x.IsHidden != true && x.Type == Type).OrderBy(x => x.NewsPosition).ToList();
                return ctb;
            }
        }
    }
}