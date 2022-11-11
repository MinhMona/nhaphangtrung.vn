using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHST.Models;
using NHST.Bussiness;
using MB.Extensions;
using WebUI.Business;
using System.Data;
using System.Data.Entity;

namespace NHST.Controllers
{
    public class AccountController
    {
        #region CRUD
        public static string Insert(string Username, string Email, string Password, int RoleID, int LevelID, int VIPLevel, int Status, int SaleID, int DathangID, DateTime CreatedDate, string CreatedBy, DateTime ModifiedDate, string ModifiedBy, string Token)
        {
            using (var dbe = new NHSTEntities()) //now wrapping the context in a using to ensure it is disposed
            {
                tbl_Account user = new tbl_Account();
                user.Username = Username;
                user.Email = Email;
                user.Token = Token;
                string Key = Password.Insert(2, Token);
                user.Password = PJUtils.Encrypt(Key, Password);
                user.RoleID = RoleID;
                user.LevelID = LevelID;
                user.VIPLevel = VIPLevel;
                user.Status = Status;
                user.Wallet = 0;
                user.SaleID = SaleID;
                user.Deposit = 0;
                user.DathangID = DathangID;
                user.Currency = 0;
                user.FeeBuyPro = "";
                user.FeeTQVNPerWeight = "";
                user.CreatedDate = CreatedDate;
                user.CreatedBy = CreatedBy;
                user.ModifiedBy = ModifiedBy;
                user.ModifiedDate = ModifiedDate;
                dbe.tbl_Account.Add(user);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                int kq = dbe.SaveChanges();
                string k = user.ID.ToString();
                return k;
            }

        }

        public static tbl_Account InsertNew(string Username, string Email, string Password, int RoleID, int LevelID, int VIPLevel, int Status, int SaleID, int DathangID, DateTime CreatedDate, string CreatedBy, DateTime ModifiedDate, string ModifiedBy, string Token)
        {
            using (var dbe = new NHSTEntities()) //now wrapping the context in a using to ensure it is disposed
            {

                tbl_Account user = new tbl_Account();
                user.Username = Username;
                user.Email = Email;
                string Key = Password.Insert(2, Token);
                user.Password = PJUtils.Encrypt(Key, Password);
                user.Token = Token;
                user.RoleID = RoleID;
                user.LevelID = LevelID;
                user.VIPLevel = VIPLevel;
                user.Status = Status;
                user.Wallet = 0;
                user.SaleID = SaleID;
                user.DathangID = DathangID;
                user.Currency = 0;
                user.FeeBuyPro = "";
                user.FeeTQVNPerWeight = "";
                user.Deposit = 0;
                user.CreatedDate = CreatedDate;
                user.CreatedBy = CreatedBy;
                user.ModifiedBy = ModifiedBy;
                user.ModifiedDate = ModifiedDate;
                dbe.tbl_Account.Add(user);
                dbe.Configuration.ValidateOnSaveEnabled = false;
                dbe.SaveChanges();
                return user;
            }

        }

        public static string updateVipLevel(int ID, int VIPLevel, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.VIPLevel = VIPLevel;
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

        public static string UpdatePasswordSystem(int ID, string Password, string NewPassword)
        {
            using (var db = new NHSTEntities())
            {
                string pass = PJUtils.Encrypt("userpass", Password);
                var ac = db.tbl_Account.Where(x => x.ID == ID).FirstOrDefault();
                if (ac != null)
                {
                    if (ac.Password == pass)
                    {
                        ac.Password = PJUtils.Encrypt("userpass", NewPassword);
                        db.Configuration.ValidateOnSaveEnabled = false;
                        int kq = db.SaveChanges();
                        return kq.ToString();
                    }
                    else
                        return "fail";
                }
                else
                    return "none";
            }
        }

        public static string updateLevelID(int ID, int LevelID, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.LevelID = LevelID;
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
        public static string updateTypePerson1()
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.RoleID == 1 && (dbe.tbl_MainOder.Where(m => DbFunctions.DiffDays(m.CreatedDate, DateTime.Now) < 61 && m.Status > 1).Select(m => m.UID).ToList()).Contains(ac.ID)).ToList();
                if (a != null)
                {
                    foreach (var item in a)
                    {
                        using (var dbe1 = new NHSTEntities())
                        {
                            var u = dbe1.tbl_Account.Where(us => us.ID == item.ID).FirstOrDefault();
                            if (u != null)
                            {
                                u.TypePerson = 1;
                                dbe1.Configuration.ValidateOnSaveEnabled = false;
                                dbe1.SaveChanges();
                            }
                        }
                    }
                    return "1";
                }
                else
                    return null;
            }
        }
        public static string updateTypePerson2()
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.RoleID == 1 && (dbe.tbl_MainOder.Where(m => DbFunctions.DiffDays(m.CreatedDate, DateTime.Now) > 60 && DbFunctions.DiffDays(m.CreatedDate, DateTime.Now) < 241 && m.Status > 1).Select(m => m.UID).ToList()).Contains(ac.ID)).ToList();
                if (a != null)
                {
                    foreach (var item in a)
                    {
                        using (var dbe1 = new NHSTEntities())
                        {
                            var u = dbe1.tbl_Account.Where(us => us.ID == item.ID).FirstOrDefault();
                            if (u != null)
                            {
                                u.TypePerson = 2;
                                dbe1.Configuration.ValidateOnSaveEnabled = false;
                                dbe1.SaveChanges();
                            }
                        }
                    }
                    return "1";
                }
                else
                    return null;
            }
        }
        public static string updateTypePerson3()
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.RoleID == 1 && (dbe.tbl_MainOder.Where(m => DbFunctions.DiffDays(m.CreatedDate, DateTime.Now) > 240 && m.Status > 1).Select(m => m.UID).ToList()).Contains(ac.ID)).ToList();
                if (a != null)
                {
                    foreach (var item in a)
                    {
                        using (var dbe1 = new NHSTEntities())
                        {
                            var u = dbe1.tbl_Account.Where(us => us.ID == item.ID).FirstOrDefault();
                            if (u != null)
                            {
                                u.TypePerson = 3;
                                dbe1.Configuration.ValidateOnSaveEnabled = false;
                                dbe1.SaveChanges();
                            }
                        }
                    }
                    return "1";
                }
                else
                    return null;
            }
        }
        public static string updateWallet(int ID, double Wallet, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.Wallet = Wallet;
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
        public static string updatewarehouseFromwarehouseTo(int ID, int WarehouseFrom, int WarehouseTo)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.WarehouseFrom = WarehouseFrom;
                    a.WarehouseTo = WarehouseTo;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string updateshipping(int ID, int shipping)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.ShippingType = shipping;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string UpdateEmail(int ID, string Email)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.Email = Email;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }

        public static string updatestatus(int ID, int status, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {

                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.Status = status;
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
        public static string UpdateRole(int ID, int roleid, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {

                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.RoleID = roleid;
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
        public static string UpdateSaleID(int ID, int saleID, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {

                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.SaleID = saleID;
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
        public static string UpdateDathangID(int ID, int DathangID, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.DathangID = DathangID;
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
        public static string UpdateCSKHID(int ID, int CSID)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.CSID = CSID;                   
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdateFeeRieng(int ID, string Currency, string FeeBuyPro, string FeeTQVNPerWeight,
            double Deposit)
        {
            using (var dbe = new NHSTEntities())
            {


                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.Currency = Convert.ToDouble(Currency);
                    a.FeeBuyPro = FeeBuyPro;
                    a.FeeTQVNPerWeight = FeeTQVNPerWeight;
                    a.Deposit = Deposit;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        public static string UpdatePassword(int ID, string Password)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    string token = Password.Insert(2, a.Token);
                    a.Password = PJUtils.Encrypt(token, Password);
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                return null;
            }
        }

        public static string UpdateToken(int ID, string Token)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.Token = Token;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    int kq = dbe.SaveChanges();
                    return kq.ToString();
                }
                return null;
            }
        }

        public static string updateWalletCYN(int ID, double WalletCYN)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.WalletCYN = WalletCYN;
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        #endregion
        #region Select
        public static List<tbl_Account> GetAllNotExcept()
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_Account> las = new List<tbl_Account>();
                las = dbe.tbl_Account.ToList();
                return las;
            }
        }
        public static List<tbl_Account> GetAllSaleID(int s)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_Account> las = new List<tbl_Account>();
                las = dbe.tbl_Account.Where(a => a.SaleID == s).OrderByDescending(a => a.SaleID).ToList();
                if (las.Count > 0)
                {
                    return las;
                }
                else return null;
            }
        }
        public static List<tbl_Account> GetAll(string s)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_Account> las = new List<tbl_Account>();
                las = dbe.tbl_Account.Where(a => a.Username.Contains(s) && a.RoleID != 0).OrderByDescending(a => a.RoleID).ThenByDescending(a => a.CreatedDate).ToList();
                return las;
            }
        }
        public static List<View_UserList> GetAll_View(string s)
        {
            using (var dbe = new NHSTEntities())
            {
                List<View_UserList> las = new List<View_UserList>();
                las = dbe.View_UserList.Where(a => a.Username.Contains(s) && a.RoleID != 0).OrderByDescending(a => a.RoleID).ThenByDescending(a => a.CreatedDate).ToList();
                return las;
            }
        }
        public static List<View_UserListWithWallet> GetAllWithWallet_View(string s)
        {
            using (var dbe = new NHSTEntities())
            {
                List<View_UserListWithWallet> las = new List<View_UserListWithWallet>();
                las = dbe.View_UserListWithWallet.Where(a => a.Username.Contains(s) && a.RoleID != 0).OrderByDescending(a => a.RoleID).ThenByDescending(a => a.CreatedDate).ToList();
                return las;
            }
        }
        public static List<View_UserListExcel> GetAll_ViewUserListExcel(string s)
        {
            using (var dbe = new NHSTEntities())
            {
                List<View_UserListExcel> las = new List<View_UserListExcel>();
                las = dbe.View_UserListExcel.Where(a => a.Username.Contains(s) && a.RoleID != 0).OrderByDescending(a => a.ID).ToList();
                return las;
            }
        }
        public static List<tbl_Account> GetUserAll(string s)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_Account> las = new List<tbl_Account>();
                las = dbe.tbl_Account.Where(a => a.Username.Contains(s) && a.RoleID == 1).OrderByDescending(a => a.RoleID).ThenByDescending(a => a.CreatedDate).ToList();
                if (las.Count > 0)
                {
                    return las;
                }
                else return null;
            }
        }
        public static List<tbl_Account> GetAllOrderDesc(string s)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_Account> las = new List<tbl_Account>();
                las = dbe.tbl_Account.Where(a => a.Username.Contains(s) && a.RoleID != 0).OrderByDescending(a => a.ID).ToList();
                if (las.Count > 0)
                {
                    return las;
                }
                else return null;
            }
        }
        public static List<tbl_Account> GetAllNotSearch()
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_Account> las = new List<tbl_Account>();
                las = dbe.tbl_Account.ToList();
                if (las.Count > 0)
                {
                    return las;
                }
                else return las;
            }
        }
        public static List<tbl_Account> GetAllByRoleID(int RoleID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_Account> las = new List<tbl_Account>();
                las = dbe.tbl_Account.Where(a => a.RoleID == RoleID).ToList();
                if (las.Count > 0)
                {
                    return las;
                }
                else return las;
            }
        }
        public static List<tbl_Account> GetAllByRoleIDAndRoleID(int RoleID1, int RoleID2)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_Account> las = new List<tbl_Account>();
                las = dbe.tbl_Account.Where(a => a.RoleID == RoleID1 || a.RoleID == RoleID2).ToList();
                if (las.Count > 0)
                {
                    return las;
                }
                else return las;
            }
        }
        public static tbl_Account GetByID(int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_Account acc = dbe.tbl_Account.Where(a => a.ID == ID).FirstOrDefault();
                if (acc != null)
                    return acc;
                else
                    return null;
            }
        }
        public static List<tbl_Account> GetByLevelID(int levelID)
        {
            using (var dbe = new NHSTEntities())
            {
                var acc = dbe.tbl_Account.Where(a => a.LevelID == levelID).ToList();
                if (acc != null)
                    return acc;
                else
                    return null;
            }
        }

        public static tbl_Account GetByUsername(string Username)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_Account acc = dbe.tbl_Account.Where(a => a.Username == Username).FirstOrDefault();
                if (acc != null)
                    return acc;
                else
                    return null;
            }
        }
        public static tbl_Account GetByEmail(string Email)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_Account acc = dbe.tbl_Account.Where(a => a.Email == Email).FirstOrDefault();
                if (acc != null)
                    return acc;
                else
                    return null;
            }
        }
        public static tbl_Account Login(string Username, string Password)
        {
            using (var dbe = new NHSTEntities())
            {
                var ac = dbe.tbl_Account.Where(x => x.Username == Username).FirstOrDefault();
                if (ac != null)
                {
                    string token = Password.Insert(2, ac.Token);
                    Password = PJUtils.Encrypt(token, Password);

                    tbl_Account acc = dbe.tbl_Account.Where(a => a.Username == Username && a.Password == Password).FirstOrDefault();
                    if (acc != null)
                        return acc;
                    else
                        return null;
                }
                else
                    return null;
            }
        }
        public static tbl_Account LoginEmail(string Email, string Password)
        {
            using (var dbe = new NHSTEntities())
            {
                Password = PJUtils.Encrypt("userpass", Password);
                tbl_Account acc = dbe.tbl_Account.Where(a => a.Email == Email && a.Password == Password).FirstOrDefault();
                if (acc != null)
                    return acc;
                else
                    return null;
            }
        }
        public static tbl_Account GetByPhone(string Phone)
        {
            using (var dbe = new NHSTEntities())
            {
                var ai = dbe.tbl_AccountInfo.Where(a => a.Phone == Phone).FirstOrDefault();
                if (ai != null)
                {
                    tbl_Account acc = dbe.tbl_Account.Where(a => a.ID == ai.UID).FirstOrDefault();
                    if (acc != null)
                        return acc;
                    else
                        return null;
                }
                else
                    return null;
            }
        }
        public static string UpdateFeeRieng_new(int ID, string FeeBuyPro, string FeeTQVNPerWeight, string Currency, string Deposit)
        {
            using (var dbe = new NHSTEntities())
            {
                var a = dbe.tbl_Account.Where(ac => ac.ID == ID).FirstOrDefault();
                if (a != null)
                {
                    a.FeeBuyPro = FeeBuyPro;
                    a.Deposit = Convert.ToDouble(Deposit);
                    a.FeeTQVNPerWeight = FeeTQVNPerWeight;
                    a.Currency = Convert.ToDouble(Currency);
                    dbe.Configuration.ValidateOnSaveEnabled = false;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        #endregion
        public static string UpdatePassForgot(int ID, string Password)
        {
            using (var db = new NHSTEntities())
            {
                var ac = db.tbl_Account.Where(x => x.ID == ID).FirstOrDefault();
                if (ac != null)
                {
                    string Key = Password.Insert(2, ac.Token);
                    ac.Password = PJUtils.Encrypt(Key, Password);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    int kq = db.SaveChanges();
                    return kq.ToString();
                }
                else
                    return null;
            }
        }

        //New

        public static tbl_Account UpdateLoginStatus(int ID, string Status)
        {
            using (var db = new NHSTEntities())
            {
                tbl_Account acc = db.tbl_Account.Where(n => n.ID == ID).FirstOrDefault();
                if (acc != null)
                {
                    acc.LoginStatus = Status;
                    db.SaveChanges();
                    return acc;
                }
                else
                    return null;
            }
        }

        public static tbl_Account UpdateScanWareHouse(int ID, int WareHouseTQ, int WareHouseVN)
        {
            using (var db = new NHSTEntities())
            {
                tbl_Account acc = db.tbl_Account.Where(n => n.ID == ID).FirstOrDefault();
                if (acc != null)
                {
                    acc.WareHouseTQ = WareHouseTQ;
                    acc.WareHouseVN = WareHouseVN;
                    db.SaveChanges();
                    return acc;
                }
                else
                    return null;
            }
        }

        public static List<View_UserList> GetUserListBySQL(string searchname, int pageIndex, int pageSize)
        {
            var sql = @"select* ";
            sql += "from View_UserList ";
            sql += "where RoleID = 1 ";
            if (!string.IsNullOrEmpty(searchname))
                sql += "and Username Like N'%" + searchname + "%'";
            sql += "order by ID desc, CreatedDate desc OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<View_UserList> list = new List<View_UserList>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new View_UserList();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();
                if (reader["FirstName"] != DBNull.Value)
                    entity.FirstName = reader["FirstName"].ToString();
                if (reader["LastName"] != DBNull.Value)
                    entity.LastName = reader["LastName"].ToString();
                if (reader["MobilePhone"] != DBNull.Value)
                    entity.MobilePhone = reader["MobilePhone"].ToString();
                if (reader["Email"] != DBNull.Value)
                    entity.Email = reader["Email"].ToString();
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);
                if (reader["Wallet"] != DBNull.Value)
                    entity.Wallet = reader["Wallet"].ToString().ToFloat(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["RoleID"] != DBNull.Value)
                    entity.RoleID = reader["RoleID"].ToString().ToInt(0);
                if (reader["RoleName"] != DBNull.Value)
                    entity.RoleName = reader["RoleName"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static int GetTotalUser(string searchName)
        {
            var sql = @"select Total=COUNT(*) ";
            sql += "from View_UserList ";
            sql += "where  RoleID=1  and Username like N'%" + searchName + "%' or ID like N'%" + searchName + "%'";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int a = 0;
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }
        public static int GetTotalUser_Thien(string searchname)
        {
            var sql = @"select Total=COUNT(*) ";
            sql += "from View_UserList ";
            sql += "where  RoleID=1 ";
            sql += "and ( concat(FirstName,' ', LastName) Like N'%" + searchname + "%' or Username Like N'%" + searchname + "%' ";
            sql += "or Email Like N'%" + searchname + "%'";
            sql += "or Phone Like N'%" + searchname + "%' )";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int a = 0;
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }

        public static List<View_AdminList> GetListStaffAdmin(string searchname, int pageIndex, int pageSize, string phone, int Status)
        {
            var sql = @"select* ";
            sql += "from View_AdminList ";
            sql += "where Username Like N'%" + searchname + "%' AND Phone Like N'%" + phone + "%'";  
            if (Status > -1)
                sql += " AND Status =" + Status + "";
            sql += "order by ID desc OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<View_AdminList> list = new List<View_AdminList>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new View_AdminList();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();
                if (reader["FirstName"] != DBNull.Value)
                    entity.FirstName = reader["FirstName"].ToString();
                if (reader["LastName"] != DBNull.Value)
                    entity.LastName = reader["LastName"].ToString();
                if (reader["MobilePhone"] != DBNull.Value)
                    entity.MobilePhone = reader["MobilePhone"].ToString();
                if (reader["Email"] != DBNull.Value)
                    entity.Email = reader["Email"].ToString();
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);
                if (reader["Wallet"] != DBNull.Value)
                    entity.Wallet = reader["Wallet"].ToString().ToFloat(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["RoleID"] != DBNull.Value)
                    entity.RoleID = reader["RoleID"].ToString().ToInt(0);
                if (reader["RoleName"] != DBNull.Value)
                    entity.RoleName = reader["RoleName"].ToString();

                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<View_UserList> GetListStaffBySQL(string searchname, int pageIndex, int pageSize, string phone, int Status, int Role)
        {
            var sql = @"select* ";
            sql += "from View_UserList ";
            sql += "where Username Like N'%" + searchname + "%' and Phone Like N'%" + phone + "%'";
            if (Role > -1)
                sql += " and RoleID =" + Role + " ";
            else
                sql += " and RoleID not in(1,0) ";

            if (Status > -1)
                sql += " and Status =" + Status + "";

            sql += "order by RoleID desc, CreatedDate desc OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<View_UserList> list = new List<View_UserList>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new View_UserList();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();
                if (reader["FirstName"] != DBNull.Value)
                    entity.FirstName = reader["FirstName"].ToString();
                if (reader["LastName"] != DBNull.Value)
                    entity.LastName = reader["LastName"].ToString();
                if (reader["MobilePhone"] != DBNull.Value)
                    entity.MobilePhone = reader["MobilePhone"].ToString();
                if (reader["Email"] != DBNull.Value)
                    entity.Email = reader["Email"].ToString();
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);
                if (reader["Wallet"] != DBNull.Value)
                    entity.Wallet = reader["Wallet"].ToString().ToFloat(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["RoleID"] != DBNull.Value)
                    entity.RoleID = reader["RoleID"].ToString().ToInt(0);
                if (reader["RoleName"] != DBNull.Value)
                    entity.RoleName = reader["RoleName"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<View_UserList> GetStaffListBySQL(string searchname, int pageIndex, int pageSize)
        {
            var sql = @"select* ";
            sql += "from View_UserList ";
            sql += "where RoleID not in(1,0) ";
            if (!string.IsNullOrEmpty(searchname))
                sql += "and Username Like N'%" + searchname + "%'";
            sql += "order by ID desc, CreatedDate desc OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<View_UserList> list = new List<View_UserList>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new View_UserList();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();
                if (reader["FirstName"] != DBNull.Value)
                    entity.FirstName = reader["FirstName"].ToString();
                if (reader["LastName"] != DBNull.Value)
                    entity.LastName = reader["LastName"].ToString();
                if (reader["MobilePhone"] != DBNull.Value)
                    entity.MobilePhone = reader["MobilePhone"].ToString();
                if (reader["Email"] != DBNull.Value)
                    entity.Email = reader["Email"].ToString();
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);
                if (reader["Wallet"] != DBNull.Value)
                    entity.Wallet = reader["Wallet"].ToString().ToFloat(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["RoleID"] != DBNull.Value)
                    entity.RoleID = reader["RoleID"].ToString().ToInt(0);
                if (reader["RoleName"] != DBNull.Value)
                    entity.RoleName = reader["RoleName"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static int GetTotalAdminStaff(string searchName, string phone, int Status)
        {
            var sql = @"select Total=COUNT(*) ";
            sql += "from View_AdminList ";
            sql += "where Username like N'%" + searchName + "%' AND Phone like N'%" + phone + "%'";           
            if (Status > -1)
            {
                sql += " AND Status =" + Status + "";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int a = 0;
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }

        public static int GetTotalStaff(string searchName, string phone, int Status, int Role)
        {
            var sql = @"select Total=COUNT(*) ";
            sql += "from View_UserList ";
            sql += "where  Username like N'%" + searchName + "%' and Phone like N'%" + phone + "%'";

            if (Role > -1)
            {
                sql += " And RoleID =" + Role + "";

            }
            else
            {
                sql += " And RoleID not in(1,0) ";
            }

            if (Status > -1)
            {
                sql += " And Status =" + Status + "";
            }

            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int a = 0;
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }

        public static List<View_UserList> GetListUserBySQL(string searchname, int pageIndex, int pageSize)
        {
            var sql = @"select* ";
            sql += "from View_UserList ";
            sql += "where  RoleID !=0 and RoleID=1 ";
            if (!string.IsNullOrEmpty(searchname))
                sql += "and Username Like N'%" + searchname + "%' or ID like N'%" + searchname + "%'";
            sql += "order by RoleID desc, CreatedDate desc OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<View_UserList> list = new List<View_UserList>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new View_UserList();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();
                if (reader["FirstName"] != DBNull.Value)
                    entity.FirstName = reader["FirstName"].ToString();
                if (reader["LastName"] != DBNull.Value)
                    entity.LastName = reader["LastName"].ToString();
                if (reader["MobilePhone"] != DBNull.Value)
                    entity.MobilePhone = reader["MobilePhone"].ToString();



                if (reader["Address"] != DBNull.Value)
                    entity.Address = reader["Address"].ToString();

                if (reader["FeeBuyPro"] != DBNull.Value)
                    entity.FeeBuyPro = reader["FeeBuyPro"].ToString();

                if (reader["FeeTQVNPerWeight"] != DBNull.Value)
                    entity.FeeTQVNPerWeight = reader["FeeTQVNPerWeight"].ToString();

                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString().ToFloat(0);

                if (reader["Currency"] != DBNull.Value)
                    entity.Currency = reader["Currency"].ToString().ToFloat(0);

                if (reader["SaleID"] != DBNull.Value)
                    entity.SaleID = reader["SaleID"].ToString().ToInt(0);






                if (reader["Email"] != DBNull.Value)
                    entity.Email = reader["Email"].ToString();
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);
                if (reader["Wallet"] != DBNull.Value)
                    entity.Wallet = reader["Wallet"].ToString().ToFloat(0);
                if (reader["WalletCYN"] != DBNull.Value)
                    entity.WalletCYN = reader["WalletCYN"].ToString().ToInt(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["RoleID"] != DBNull.Value)
                    entity.RoleID = reader["RoleID"].ToString().ToInt(0);
                if (reader["RoleName"] != DBNull.Value)
                    entity.RoleName = reader["RoleName"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<View_UserList> GetListUserBySQL_Excel_Thien(string searchname)
        {
            var sql = @"select* ";
            sql += "from View_UserList ";
            sql += "where RoleID=1 ";
            if (!string.IsNullOrEmpty(searchname))
            {
                sql += "and ( Username Like N'%" + searchname + "%'";
                sql += "or Email Like N'%" + searchname + "%'";
                sql += "or Phone Like N'%" + searchname + "%' )";
            }

            //sql += "order by RoleID desc, CreatedDate desc OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<View_UserList> list = new List<View_UserList>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new View_UserList();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();
                if (reader["FirstName"] != DBNull.Value)
                    entity.FirstName = reader["FirstName"].ToString();
                if (reader["LastName"] != DBNull.Value)
                    entity.LastName = reader["LastName"].ToString();
                if (reader["MobilePhone"] != DBNull.Value)
                    entity.MobilePhone = reader["MobilePhone"].ToString();



                if (reader["Address"] != DBNull.Value)
                    entity.Address = reader["Address"].ToString();

                if (reader["FeeBuyPro"] != DBNull.Value)
                    entity.FeeBuyPro = reader["FeeBuyPro"].ToString();

                if (reader["FeeTQVNPerWeight"] != DBNull.Value)
                    entity.FeeTQVNPerWeight = reader["FeeTQVNPerWeight"].ToString();

                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString().ToFloat(0);

                if (reader["Currency"] != DBNull.Value)
                    entity.Currency = reader["Currency"].ToString().ToFloat(0);

                if (reader["SaleID"] != DBNull.Value)
                    entity.SaleID = reader["SaleID"].ToString().ToInt(0);






                if (reader["Email"] != DBNull.Value)
                    entity.Email = reader["Email"].ToString();
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);
                if (reader["Wallet"] != DBNull.Value)
                    entity.Wallet = reader["Wallet"].ToString().ToFloat(0);
                if (reader["WalletCYN"] != DBNull.Value)
                    entity.WalletCYN = reader["WalletCYN"].ToString().ToInt(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["RoleID"] != DBNull.Value)
                    entity.RoleID = reader["RoleID"].ToString().ToInt(0);
                if (reader["RoleName"] != DBNull.Value)
                    entity.RoleName = reader["RoleName"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static List<View_UserList> GetListUserBySQL_Thien(string searchname, int pageIndex, int pageSize)
        {
            var sql = @"select* ";
            sql += "from View_UserList ";
            sql += "where RoleID=1 ";
            if (!string.IsNullOrEmpty(searchname))
            {
                sql += "and ( concat(FirstName,' ', LastName) Like N'%" + searchname + "%' or Username Like N'%" + searchname + "%' ";
                sql += " or Email Like N'%" + searchname + "%'";
                sql += " or Phone Like N'%" + searchname + "%' ) ";
            }
            sql += "order by RoleID desc, CreatedDate desc OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<View_UserList> list = new List<View_UserList>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new View_UserList();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();
                if (reader["FirstName"] != DBNull.Value)
                    entity.FirstName = reader["FirstName"].ToString();
                if (reader["LastName"] != DBNull.Value)
                    entity.LastName = reader["LastName"].ToString();
                if (reader["Phone"] != DBNull.Value)
                    entity.Phone = reader["Phone"].ToString();

                if (reader["Address"] != DBNull.Value)
                    entity.Address = reader["Address"].ToString();

                if (reader["FeeBuyPro"] != DBNull.Value)
                    entity.FeeBuyPro = reader["FeeBuyPro"].ToString();

                if (reader["FeeTQVNPerWeight"] != DBNull.Value)
                    entity.FeeTQVNPerWeight = reader["FeeTQVNPerWeight"].ToString();

                if (reader["Deposit"] != DBNull.Value)
                    entity.Deposit = reader["Deposit"].ToString().ToFloat(0);

                if (reader["Currency"] != DBNull.Value)
                    entity.Currency = reader["Currency"].ToString().ToFloat(0);

                if (reader["LevelID"] != DBNull.Value)
                    entity.LevelID = reader["LevelID"].ToString().ToInt(0);
                
                if (reader["SaleID"] != DBNull.Value)
                    entity.SaleID = reader["SaleID"].ToString().ToInt(0);

                if (reader["Email"] != DBNull.Value)
                    entity.Email = reader["Email"].ToString();
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);
                if (reader["Wallet"] != DBNull.Value)
                    entity.Wallet = reader["Wallet"].ToString().ToFloat(0);
                if (reader["WalletCYN"] != DBNull.Value)
                    entity.WalletCYN = reader["WalletCYN"].ToString().ToInt(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["RoleID"] != DBNull.Value)
                    entity.RoleID = reader["RoleID"].ToString().ToInt(0);
                if (reader["RoleName"] != DBNull.Value)
                    entity.RoleName = reader["RoleName"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static double GetTotalWalletBySQL(string valFrom, string valTo)
        {
            var sql = @"select Total=SUM(Wallet) ";
            sql += "from View_UserList ";
            if (!string.IsNullOrEmpty(valFrom))
            {
                sql += "where Wallet>=" + valFrom + "  ";
                if (!string.IsNullOrEmpty(valTo))
                {
                    sql += "and Wallet<=" + valTo + " ";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(valTo))
                {
                    sql += "Where Wallet<=" + valTo + " ";
                }
            }
            double a = 0;
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = Convert.ToDouble(reader["Total"].ToString());
            }
            reader.Close();
            return a;
        }
        public static double GetTotalWalletDesc()
        {
            var sql = @"select Total=SUM(Wallet) ";
            sql += "from View_UserList ";           
            double a = 0;
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = Convert.ToDouble(reader["Total"].ToString());
            }
            reader.Close();
            return a;
        }
        public static int GetTotalBySQL(string valFrom, string valTo)
        {
            var sql = @"select Total=Count(*) ";
            sql += "from View_UserList ";
            if (!string.IsNullOrEmpty(valFrom))
            {
                sql += "where Wallet>=" + valFrom + "  ";
                if (!string.IsNullOrEmpty(valTo))
                {
                    sql += "and Wallet<=" + valTo + " ";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(valTo))
                {
                    sql += "Where Wallet<=" + valTo + " ";
                }
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
        public static int GetTotalOrderByWalletDesc()
        {
            var sql = @"select Total=Count(*) ";
            sql += "from View_UserList ";           
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
        public static List<View_UserList> GetAll_View_ButBySQL(string valFrom, string valTo, int pageSize, int pageIndex)
        {
            var sql = @"select * ";
            sql += "from View_UserList ";
            if (!string.IsNullOrEmpty(valFrom))
            {
                sql += "where Wallet>=" + valFrom + "  ";
                if (!string.IsNullOrEmpty(valTo))
                {
                    sql += "and Wallet<=" + valTo + " ";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(valTo))
                {
                    sql += "Where Wallet<=" + valTo + " ";
                }
            }
            sql += " order by RoleID desc, CreatedDate desc OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<View_UserList> list = new List<View_UserList>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new View_UserList();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();
                if (reader["FirstName"] != DBNull.Value)
                    entity.FirstName = reader["FirstName"].ToString();
                if (reader["LastName"] != DBNull.Value)
                    entity.LastName = reader["LastName"].ToString();
                if (reader["MobilePhone"] != DBNull.Value)
                    entity.MobilePhone = reader["MobilePhone"].ToString();
                if (reader["Email"] != DBNull.Value)
                    entity.Email = reader["Email"].ToString();
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);
                if (reader["Wallet"] != DBNull.Value)
                    entity.Wallet = reader["Wallet"].ToString().ToFloat(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["RoleID"] != DBNull.Value)
                    entity.RoleID = reader["RoleID"].ToString().ToInt(0);
                if (reader["RoleName"] != DBNull.Value)
                    entity.RoleName = reader["RoleName"].ToString();
                if (reader["saler"] != DBNull.Value)
                    entity.saler = reader["saler"].ToString();
                if (reader["dathang"] != DBNull.Value)
                    entity.dathang = reader["dathang"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<View_UserList> GetAllOrderByWalletDesc(int pageSize, int pageIndex)
        {
            var sql = @"select * ";
            sql += "from View_UserList ";           
            sql += "order by Wallet desc OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<View_UserList> list = new List<View_UserList>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new View_UserList();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();
                if (reader["FirstName"] != DBNull.Value)
                    entity.FirstName = reader["FirstName"].ToString();
                if (reader["LastName"] != DBNull.Value)
                    entity.LastName = reader["LastName"].ToString();
                if (reader["MobilePhone"] != DBNull.Value)
                    entity.MobilePhone = reader["MobilePhone"].ToString();
                if (reader["Email"] != DBNull.Value)
                    entity.Email = reader["Email"].ToString();
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);
                if (reader["Wallet"] != DBNull.Value)
                    entity.Wallet = reader["Wallet"].ToString().ToFloat(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["RoleID"] != DBNull.Value)
                    entity.RoleID = reader["RoleID"].ToString().ToInt(0);
                if (reader["RoleName"] != DBNull.Value)
                    entity.RoleName = reader["RoleName"].ToString();
                if (reader["saler"] != DBNull.Value)
                    entity.saler = reader["saler"].ToString();
                if (reader["dathang"] != DBNull.Value)
                    entity.dathang = reader["dathang"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static List<View_UserList> GetAllOrderByWallet(int pageSize, int pageIndex)
        {
            var sql = @"select * ";
            sql += "from View_UserList ";
            sql += "order by Wallet OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<View_UserList> list = new List<View_UserList>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new View_UserList();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();
                if (reader["FirstName"] != DBNull.Value)
                    entity.FirstName = reader["FirstName"].ToString();
                if (reader["LastName"] != DBNull.Value)
                    entity.LastName = reader["LastName"].ToString();
                if (reader["MobilePhone"] != DBNull.Value)
                    entity.MobilePhone = reader["MobilePhone"].ToString();
                if (reader["Email"] != DBNull.Value)
                    entity.Email = reader["Email"].ToString();
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);
                if (reader["Wallet"] != DBNull.Value)
                    entity.Wallet = reader["Wallet"].ToString().ToFloat(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["RoleID"] != DBNull.Value)
                    entity.RoleID = reader["RoleID"].ToString().ToInt(0);
                if (reader["RoleName"] != DBNull.Value)
                    entity.RoleName = reader["RoleName"].ToString();
                if (reader["saler"] != DBNull.Value)
                    entity.saler = reader["saler"].ToString();
                if (reader["dathang"] != DBNull.Value)
                    entity.dathang = reader["dathang"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }
        public static int GetTotalStaffOfSaler(string searchName, int SalerID)
        {
            var sql = @"select Total=COUNT(*) ";
            sql += "from View_UserList ";
            sql += "where  RoleID =1 And (SaleID = 0) and Username like N'%" + searchName + "%'";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int a = 0;
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }
        public static int GetTotalStaffOfCSKH(string searchName, int SalerID)
        {
            var sql = @"select Total=COUNT(*) ";
            sql += "from View_UserList ";
            sql += "where RoleID=1 and CSID = " + SalerID + " ";
            if (!string.IsNullOrEmpty(searchName))
                sql += " and Username Like N'%" + searchName + "%'";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int a = 0;
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }
        public static int GetTotalStaffOfSale(string searchName, int SalerID)
        {
            var sql = @"select Total=COUNT(*) ";
            sql += "from View_UserList ";
            sql += "where  RoleID =1 And  SaleID = " + SalerID + "  and Username like N'%" + searchName + "%'";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int a = 0;
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);
            }
            reader.Close();
            return a;
        }
        public static List<View_UserList> GetListStaffOfSalerBySQL(string searchname, int SalerID, int pageIndex, int pageSize)
        {
            var sql = @"select* ";
            sql += "from View_UserList ";
            sql += "where RoleID = 1 And (SaleID = 0) ";
            if (!string.IsNullOrEmpty(searchname))
                sql += "and Username Like N'%" + searchname + "%'";
            sql += "order by ID desc, CreatedDate desc OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<View_UserList> list = new List<View_UserList>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new View_UserList();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();
                if (reader["FirstName"] != DBNull.Value)
                    entity.FirstName = reader["FirstName"].ToString();
                if (reader["LastName"] != DBNull.Value)
                    entity.LastName = reader["LastName"].ToString();
                if (reader["MobilePhone"] != DBNull.Value)
                    entity.MobilePhone = reader["MobilePhone"].ToString();
                if (reader["Email"] != DBNull.Value)
                    entity.Email = reader["Email"].ToString();
                if (reader["Address"] != DBNull.Value)
                    entity.Address = reader["Address"].ToString();
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);
                if (reader["Wallet"] != DBNull.Value)
                    entity.Wallet = reader["Wallet"].ToString().ToFloat(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["RoleID"] != DBNull.Value)
                    entity.RoleID = reader["RoleID"].ToString().ToInt(0);
                if (reader["SaleID"] != DBNull.Value)
                    entity.SaleID = reader["SaleID"].ToString().ToInt(0);
                if (reader["RoleName"] != DBNull.Value)
                    entity.RoleName = reader["RoleName"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<View_UserList> GetListStaffOfSaleBySQL(string searchname, int SalerID, int pageIndex, int pageSize)
        {
            var sql = @"select* ";
            sql += "from View_UserList ";
            sql += "where RoleID = 1 And  SaleID = " + SalerID + " ";
            if (!string.IsNullOrEmpty(searchname))
                sql += "and Username Like N'%" + searchname + "%'";
            sql += "order by ID desc, CreatedDate desc OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<View_UserList> list = new List<View_UserList>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new View_UserList();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();
                if (reader["FirstName"] != DBNull.Value)
                    entity.FirstName = reader["FirstName"].ToString();
                if (reader["LastName"] != DBNull.Value)
                    entity.LastName = reader["LastName"].ToString();
                if (reader["MobilePhone"] != DBNull.Value)
                    entity.MobilePhone = reader["MobilePhone"].ToString();
                if (reader["Email"] != DBNull.Value)
                    entity.Email = reader["Email"].ToString();
                if (reader["Address"] != DBNull.Value)
                    entity.Address = reader["Address"].ToString();
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);
                if (reader["Wallet"] != DBNull.Value)
                    entity.Wallet = reader["Wallet"].ToString().ToFloat(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["RoleID"] != DBNull.Value)
                    entity.RoleID = reader["RoleID"].ToString().ToInt(0);
                if (reader["SaleID"] != DBNull.Value)
                    entity.SaleID = reader["SaleID"].ToString().ToInt(0);
                if (reader["RoleName"] != DBNull.Value)
                    entity.RoleName = reader["RoleName"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static List<View_UserList> GetListStaffOfCSKH(string searchname, int SalerID, int pageIndex, int pageSize)
        {
            var sql = @"select* ";
            sql += "from View_UserList ";
            sql += "where RoleID=1 and CSID = " + SalerID + " ";
            if (!string.IsNullOrEmpty(searchname))
                sql += "and Username Like N'%" + searchname + "%'";
            sql += "order by ID desc, CreatedDate desc OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY ";
            List<View_UserList> list = new List<View_UserList>();
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            while (reader.Read())
            {
                var entity = new View_UserList();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["Username"] != DBNull.Value)
                    entity.Username = reader["Username"].ToString();
                if (reader["FirstName"] != DBNull.Value)
                    entity.FirstName = reader["FirstName"].ToString();
                if (reader["LastName"] != DBNull.Value)
                    entity.LastName = reader["LastName"].ToString();
                if (reader["MobilePhone"] != DBNull.Value)
                    entity.MobilePhone = reader["MobilePhone"].ToString();
                if (reader["Email"] != DBNull.Value)
                    entity.Email = reader["Email"].ToString();
                if (reader["Address"] != DBNull.Value)
                    entity.Address = reader["Address"].ToString();
                if (reader["Status"] != DBNull.Value)
                    entity.Status = reader["Status"].ToString().ToInt(0);
                if (reader["Wallet"] != DBNull.Value)
                    entity.Wallet = reader["Wallet"].ToString().ToFloat(0);
                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["RoleID"] != DBNull.Value)
                    entity.RoleID = reader["RoleID"].ToString().ToInt(0);
                if (reader["SaleID"] != DBNull.Value)
                    entity.SaleID = reader["SaleID"].ToString().ToInt(0);
                if (reader["RoleName"] != DBNull.Value)
                    entity.RoleName = reader["RoleName"].ToString();
                list.Add(entity);
            }
            reader.Close();
            return list;
        }

        public static int UserWallet_Auto(int UID, double Amount, int HpID, string Note)
        {
            using (NHSTEntities dbe = new NHSTEntities())
            {
                dbe.Configuration.ValidateOnSaveEnabled = false;
                using (var transaction = dbe.Database.BeginTransaction())
                {
                    try
                    {
                        var ac = AccountController.GetByID(UID);
                        if (ac != null)
                        {
                            double walletleft = Convert.ToDouble(ac.Wallet) + Amount;

                            var a = dbe.tbl_Account.Where(x => x.ID == ac.ID).FirstOrDefault();
                            if (a != null)
                            {
                                a.Wallet = walletleft;
                            }
                            //ac.Wallet = walletleft;

                            tbl_AdminSendUserWallet acc = new tbl_AdminSendUserWallet();
                            acc.UID = UID;
                            acc.Username = ac.Username;
                            acc.Amount = Amount;
                            acc.Status = 2;
                            acc.BankID = 100;
                            acc.TradeContent = "Nạp tiền tự động - " + Note + "";
                            acc.CreatedDate = DateTime.Now;
                            acc.CreatedBy = ac.Username;
                            dbe.tbl_AdminSendUserWallet.Add(acc);

                            tbl_HistoryPayWallet payWallet = new tbl_HistoryPayWallet();
                            payWallet.UID = UID;
                            payWallet.UserName = acc.Username;
                            payWallet.MainOrderID = 0;
                            payWallet.Amount = Amount;
                            payWallet.HContent = "Nạp tiền tự động - " + Note + "";
                            payWallet.MoneyLeft = walletleft;
                            payWallet.Type = 2;
                            payWallet.TradeType = 4;
                            payWallet.CreatedDate = DateTime.Now;
                            payWallet.CreatedBy = ac.Username;
                            dbe.tbl_HistoryPayWallet.Add(payWallet);

                            var checkhis = dbe.tbl_HistoryAutoBanking.Where(x => x.UID == UID && x.Note == Note).FirstOrDefault();
                            if (checkhis != null)
                            {
                                checkhis.Status = 2;
                            }
                        }

                        string url = "http://tt.mona.media/paymentservice.asmx/UpdateStatus?UID=4&Key=monasms-autobanking&PayID=" + HpID + "";
                        var kq = PJUtils.ConnectApi(url);

                        dbe.SaveChanges();
                        transaction.Commit();
                        return 1;

                    }
                    catch
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}