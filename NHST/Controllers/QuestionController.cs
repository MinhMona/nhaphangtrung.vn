using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHST.Controllers
{
    public class QuestionController
    {
        public static tbl_Question GetByID(int ID)
        {
            using (var db = new NHSTEntities())
            {
                var sv = db.tbl_Question.Where(x => x.ID == ID).FirstOrDefault();
                if (sv != null)
                    return sv;
                return null;
            }
        }
        public static List<tbl_Question> GetAll()
        {
            using (var db = new NHSTEntities())
            {
                List<tbl_Question> ctb = new List<tbl_Question>();
                ctb = db.tbl_Question.OrderBy(x => x.QuesPosition).ToList();
                return ctb;
            }
        }
        public static List<tbl_Question> GetAllNotHiden()
        {
            using (var db = new NHSTEntities())
            {
                List<tbl_Question> ctb = new List<tbl_Question>();
                ctb = db.tbl_Question.Where(x => x.IsHiden != true).OrderBy(x => x.QuesPosition).ToList();
                return ctb;
            }
        }
        public static tbl_Question Update(int ID, string QuesTitle, string QuesDesc, string NewsLink, bool IsHidden, string QuesIMG, int QuesPosition, string Created)
        {
            using (var db = new NHSTEntities())
            {
                var sv = db.tbl_Question.Where(x => x.ID == ID).FirstOrDefault();
                if (sv != null)
                {
                    sv.QuesTitle = QuesTitle;
                    sv.QuesDesc = QuesDesc;
                    sv.QuesLink = NewsLink;
                    sv.IsHiden = IsHidden;
                    if (!string.IsNullOrEmpty(QuesIMG))
                        sv.QuesIMG = QuesIMG;
                    sv.QuesPosition = QuesPosition;
                    sv.ModifiedBy = Created;
                    sv.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    return sv;
                }
                return null;
            }
        }
    }
}