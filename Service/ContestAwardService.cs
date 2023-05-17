using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;

namespace LabWeb.Service
{
    public class ContestAwardService
    {
        private readonly SqlConnection conn;

        public ContestAwardService(SqlConnection connection)
        {
            conn = connection;
        }

        public IEnumerable<Contest_Award> GetAllData()
        {
            string sql = $@"SELECT * FROM Contest_Award 
                            
                            WHERE is_delete = 0;";
            var DataList = new List<Contest_Award>();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    Contest_Award Data = new Contest_Award();
                    Data.contest_id = (Guid)dr["contest_id"];
                    Data.contest_name = dr["contest_name"].ToString();
                    Data.contest_rank = dr["contest_rank"].ToString();
                    Data.contest_work = dr["contest_work"].ToString();
                    Data.contest_year = Convert.ToInt32(dr["contest_year"]);
                    DataList.Add(Data);
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

            return DataList;
        }

        public void InsertContestAward(Contest_Award newData)
        {
            string sql = $@"INSERT INTO Contest_Award
                            (contest_id,contest_year,contest_name,contest_work,contest_rank,
                            create_time,create_id,update_time,update_id,is_delete) 
                            VALUES
                            (@contest_id, @contest_year, @contest_name,@contest_work, 
                            @contest_rank, @create_time, @create_id, @update_time, @update_id,0);";
            
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.contest_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@contest_id", newData.contest_id);
                cmd.Parameters.AddWithValue("@contest_year", newData.contest_year);
                cmd.Parameters.AddWithValue("@contest_name", newData.contest_name);
                cmd.Parameters.AddWithValue("@contest_work", newData.contest_work);
                cmd.Parameters.AddWithValue("@contest_rank", newData.contest_rank);
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

        public Contest_Award GetDataById(Guid Id)
        {
            string sql = $@"SELECT * FROM Contest_Award 
                            WHERE contest_id = @Id AND is_delete=0;";

            Contest_Award Data = new Contest_Award();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.contest_id = (Guid)dr["contest_id"];
                Data.contest_name = dr["contest_name"].ToString();
                Data.contest_rank = dr["contest_rank"].ToString();
                Data.contest_work = dr["contest_work"].ToString();
                Data.contest_year = Convert.ToInt32(dr["contest_year"]);

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

        public void UpdateContestAward(Contest_Award updateData)
        {
            string sql = $@"UPDATE Contest_Award 
                            SET 
                            contest_name = @contest_name, contest_rank = @contest_rank,contest_work = @contest_work, 
                            contest_year = @contest_year,update_time = @update_time,update_id = @update_id 
                            WHERE 
                            contest_id = @Id;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.contest_id);
                cmd.Parameters.AddWithValue("@contest_name", updateData.contest_name);
                cmd.Parameters.AddWithValue("@contest_rank", updateData.contest_rank);
                cmd.Parameters.AddWithValue("@contest_work", updateData.contest_work);
                cmd.Parameters.AddWithValue("@contest_year", updateData.contest_year);
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

        public void SoftDeleteContestAwardById(Guid id)
        {
            string sql = $@"UPDATE Contest_Award SET is_delete = 1 WHERE contest_id = @Id;";

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