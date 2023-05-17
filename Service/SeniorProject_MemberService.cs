using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;

namespace LabWeb.Service
{
    public class SeniorProject_MemberService
    {
        private readonly SqlConnection conn;

        public SeniorProject_MemberService(SqlConnection connection)
        {
            conn = connection;
        }
        public IEnumerable<SeniorProject_Member> GetAllData()
        {
            string sql = $@"SELECT m.*,r.name FROM SeniorProject_Member m
                            INNER JOIN Members r ON m.members_id = r.members_id
                            WHERE m.is_delete = 0;";
            var DataDict = new Dictionary<Guid, SeniorProject_Member>();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Guid seniorproject_id = (Guid)dr["seniorproject_id"];
                    if (DataDict.ContainsKey(seniorproject_id))
                    {
                        DataDict[seniorproject_id].name += ", " + dr["name"].ToString();
                    }
                    else
                    {
                        SeniorProject_Member Data = new SeniorProject_Member();
                        Data.seniorproject_id = seniorproject_id;
                        Data.members_id = (Guid)dr["members_id"];
                        Data.name = dr["name"].ToString();
                        DataDict.Add(seniorproject_id, Data);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return DataDict.Values;
}


        public void InsertSeniorProject_Member(SeniorProject_Member newData)
        {
            string sql = $@"INSERT INTO SeniorProject_Member
                            (seniormember_id,seniorproject_id,members_id,create_time,create_id,update_time,update_id,is_delete) 
                            VALUES
                            (@seniormember_id, @seniorproject_id, @members_id, @create_time, @create_id, @update_time, @update_id, 0);";
            
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.seniormember_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@seniormember_id", newData.seniormember_id);
                cmd.Parameters.AddWithValue("@seniorproject_id", newData.seniorproject_id);
                cmd.Parameters.AddWithValue("@members_id", newData.members_id);
                cmd.Parameters.AddWithValue("@create_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@create_id", newData.create_id);
                cmd.Parameters.AddWithValue("@update_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@update_id", newData.update_id);

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

        public SeniorProject_Member GetDataById(Guid Id)
        {
            string sql = $@"SELECT m.*,r.name FROM SeniorProject_Member m 
                            INNER JOIN Members r ON m.members_id = r.members_id 
                            WHERE m.seniorproject_id = @Id AND m.is_delete = 0;";
            SeniorProject_Member Data = new SeniorProject_Member();
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr = cmd.ExecuteReader();
                StringBuilder nameBuilder = new StringBuilder();
                while (dr.Read())
                {
                    if (nameBuilder.Length > 0)
                    {
                        nameBuilder.Append(", ");
                    }
                    nameBuilder.Append(dr["name"].ToString());
                }
                Data.name = nameBuilder.ToString();

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }

        public void UpdateSeniorProject_Member(SeniorProject_Member updateData)
        {
            string sql = $@"UPDATE SeniorProject_Member 
                            SET 
                            members_id = @members_id,update_time = @update_time,update_id = @update_id 
                            WHERE 
                            seniorproject_id = @Id;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.seniorproject_id);
                cmd.Parameters.AddWithValue("@members_id", updateData.members_id);
                cmd.Parameters.AddWithValue("@update_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@update_id", updateData.update_id);
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

        public void SoftDeleteSeniorProject_MemberById(Guid id)
        {
            string sql = $@"UPDATE SeniorProject_Member SET is_delete = 1 WHERE seniorproject_id = @Id;";

            try
            {
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