using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using LabWeb.models;
using System.Data;


namespace LabWeb.Service
{
    public class MembersDBService
    {   
        private readonly SqlConnection conn;

        public MembersDBService(SqlConnection connection)
        {
            conn = connection;
        }


        #region 註冊
        #region 新增會員
        public void Register(Members newMember)
        {
            newMember.password = HashPassword(newMember.password);
            string sql = string.Empty;
            if (newMember.level == 2)
            {
                sql = $@"INSERT INTO Members (members_id, account, password, name, entry_year, authcode, email, level, create_id, 
                    create_time, update_id, update_time, is_delete) VALUES (@members_id, @account, @password, @name, @entry_year,@authcode, @email,
                        '2', @create_id, @create_time, @update_id, @update_time, @is_delete)";
            }
            else if (newMember.level == 1)
            {
               sql = $@"INSERT INTO Members (members_id, account, password, name, entry_year, authcode, email, level, create_id, 
                    create_time, update_id, update_time, is_delete) VALUES (@members_id, @account, @password, @name, @entry_year,@authcode, @email,
                        '1', @create_id, @create_time, @update_id, @update_time, @is_delete)";
            }
            else if (newMember.level == 0)
            {
                sql = $@"INSERT INTO Members (members_id, account, password, name, entry_year, authcode, email, level, create_id, 
                    create_time, update_id, update_time, is_delete) VALUES (@members_id, @account, @password, @name, @entry_year,@authcode, @email,
                        '0', @create_id, @create_time, @update_id, @update_time, @is_delete)";
            }
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newMember.members_id = Guid.NewGuid();
                newMember.create_id = Guid.NewGuid();
                newMember.update_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@Members_id", newMember.members_id);
                cmd.Parameters.AddWithValue("@account", newMember.account);
                cmd.Parameters.AddWithValue("@password", newMember.password);
                cmd.Parameters.AddWithValue("@name", newMember.name);
                cmd.Parameters.AddWithValue("@entry_year", newMember.entry_year);
                cmd.Parameters.AddWithValue("@authcode", newMember.authcode);
                cmd.Parameters.AddWithValue("@email", newMember.email);
                cmd.Parameters.AddWithValue("@level", newMember.level);
                cmd.Parameters.AddWithValue("@create_id", newMember.create_id);
                cmd.Parameters.AddWithValue("@create_time",DateTime.Now);
                cmd.Parameters.AddWithValue("@update_id", newMember.update_id);
                cmd.Parameters.AddWithValue("@update_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@is_delete", newMember.is_delete);

                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region Hash密碼
        public string HashPassword(string Password)
        {
            string saltkey = "1q2w3e4r5t6y7uu";
            string saltkeyAndPassword = string.Concat(Password, saltkey);
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] PassData = Encoding.UTF8.GetBytes(saltkeyAndPassword);
            byte[] HashData = sha256.ComputeHash(PassData);
            string Hashresult = Convert.ToBase64String(HashData);
            return Hashresult;
        }
        #endregion

        #region 查詢一筆資料
        public Members GetDataByAccount(string Account)
        {
            Members Data = new Members();
            string sql = $@"SELECT * FROM MEMBERS WHERE account='{Account}' AND is_delete=0;";
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.members_id = (Guid)dr["members_id"];
                Data.account = dr["account"].ToString();
                Data.password = dr["password"].ToString();
                Data.name = dr["name"].ToString();
                Data.entry_year = Convert.ToInt32(dr["entry_year"]);
                Data.authcode = dr["authcode"].ToString();
                Data.email = dr["email"].ToString();
                Data.level = Convert.ToInt32(dr["level"]);
            }
            catch(Exception)
            {
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }
        #endregion

        #region 帳號註冊重複確認
        public bool AccountCheck(string Account)
        {
            Members Data = GetDataByAccount(Account);
            bool result = (Data == null);
            return result;
        }
        #endregion

         #region 帳號確認
        public bool AccountExit(string Account)
        {
            Members Data = GetDataByAccount(Account);
            bool result = (Data != null);
            return result;
        }
        #endregion

        #region 信箱驗證
        public string EmailValidate(string Account,string AuthCode)
        {
            Members ValidateMember = GetDataByAccount(Account);
            string ValidateStr = string.Empty;
            if (ValidateMember != null)
            {
                if (ValidateMember.authcode == AuthCode)
                {
                    string sql = $@"UPDATE MEMBERS SET authcode = '{string.Empty}' WHERE account= @account";
                    try
                    {
                        if (conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        cmd.Parameters.AddWithValue("@account", Account);

                        cmd.ExecuteNonQuery();
                    }
                    catch(Exception e)
                    {
                        throw new Exception(e.Message.ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }
                    ValidateStr = "帳號信箱驗證成功，現在可以登入";
                }
                else
                {
                    ValidateStr = "驗證碼錯誤，請重新確認或在註冊";
                }
            }
            else
            {
                ValidateStr = "查無此帳號，請再重新註冊";
            }
            return ValidateStr;
        }
        #endregion
        
        #endregion


        #region 登入
        #region 登入確認
        public string LoginCheck(string Account,string Password)
        {
            Members LoginMember = GetDataByAccount(Account);
            if (LoginMember != null)
            {
                if (string.IsNullOrWhiteSpace(LoginMember.authcode))
                {
                    if(LoginMember.is_delete == false)
                    {
                        if (PasswordCheck(LoginMember, Password))
                        {
                            return "";
                        }
                        else
                        {
                            return "密碼錯誤";
                        }
                    }
                    else
                    {
                        return "此帳號已停用";
                    }
                }
                else
                {
                    return "尚未驗證，請去EMAIL收驗證信";
                }
            }
            else
            {
                return "無此會員，請去註冊";
            }
        }
        #endregion

        #region 密碼確認
        public bool PasswordCheck(Members CheckMember,string Password)
        {
            bool result = CheckMember.password.Equals(HashPassword(Password));
            return result;
        }
        #endregion
        
        #region 取得角色
        public string GetRole(string Account)
        {
            string Role = "Junior";
            Members LoginMember = GetDataByAccount(Account);
            if (LoginMember.level == 1)
            {
                Role = "Senior";
            }
            if (LoginMember.level == 0)
            {
                Role = "Admin";
            }
            return Role;
        }
        #endregion

        #endregion

        #region 修改密碼
        public string ChangePassword(string Account,string Password,string newPassword)
        {
            Members LoginMember = GetDataByAccount(Account);
            if (PasswordCheck(LoginMember, Password))
            {
                LoginMember.password = HashPassword(newPassword);
                string sql = $@"UPDATE MEMBERS SET password = @password WHERE account= @account ";
                try
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@password", LoginMember.password);
                    cmd.Parameters.AddWithValue("@account", Account);

                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    throw new Exception(e.Message.ToString());
                }
                finally
                {
                    conn.Close();
                }
                return "密碼修改完成";
            }
            else
            {
                return "舊密碼輸入錯誤";
            }
        }
        #endregion
    
        #region 忘記密碼
        public string ResetPassword(string account,string password)
        {
            string HashTemp = HashPassword(password);
            string sql = $@"UPDATE MEMBERS SET password = @password WHERE account= @account ";

             try
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@password", HashTemp);
                    cmd.Parameters.AddWithValue("@account", account);

                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    throw new Exception(e.Message.ToString());
                }
                finally
                {
                    conn.Close();
                }
                return HashTemp;
        }

        public string GetTempPassword()
        {
            string Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789";
            string TempPassword = "";
            Random rd = new Random();

            for (int i = 0; i < 5; i++)
            {
                TempPassword += Code[rd.Next(Code.Count())];
            }

            return TempPassword;
        }
        #endregion

        #region 查詢會員資料陣列
        public List<Members> GetDataByAccountList()
        {
            List<Members> DataList = new List<Members>();
            string sql = $@"SELECT * FROM MEMBERS WHERE is_delete=0;";
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    Members Data=new Members();
                    Data.members_id = (Guid)dr["members_id"];
                    Data.account = dr["account"].ToString();
                    Data.password = dr["password"].ToString();
                    Data.name = dr["name"].ToString();
                    Data.authcode = dr["AuthCode"].ToString();
                    Data.email = dr["email"].ToString();
                    DataList.Add(Data);
                }
            }
            catch(Exception e)
            {
                DataList=null;
            }
            finally
            {
                conn.Close();
            }
            return DataList;
        }
        #endregion

        public void ChangeMemberLevel(Members newLevel)
        {
            string sql = $@"UPDATE Members SET 
                            level = @newlevel,update_id = @update_id,update_time = @update_time 
                            WHERE members_id = @Id;";

            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", newLevel.members_id);
                cmd.Parameters.AddWithValue("@newlevel", newLevel.level);
                cmd.Parameters.AddWithValue("@update_id", newLevel.update_id);
                cmd.Parameters.AddWithValue("@update_time", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public List<Members> GetDataButOnlyIdAndName()
        {
            
            string sql = $@"SELECT members_id,name FROM MEMBERS WHERE is_delete=0;";
            var DataList = new List<Members>();
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    Members Data = new Members();
                    Data.members_id = (Guid)dr["members_id"];
                    Data.name = dr["name"].ToString();

                    DataList.Add(Data);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return DataList;
        }

        public void SoftDeleteMemberById(Guid id)
        {
            string sql = $@"UPDATE Members SET is_delete = 1 WHERE members_id = @Id;";

            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
    }
}