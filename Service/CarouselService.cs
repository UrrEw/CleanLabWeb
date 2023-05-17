using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using LabWeb.models;
using System.Data;

namespace LabWeb.Service
{
    public class CarouselService
    {
        private readonly SqlConnection conn;

        public CarouselService(SqlConnection connection)
        {
            conn = connection;
        }

        public IEnumerable<Carousel> GetAllData()
        {
            string sql = $@"SELECT * FROM Carousel WHERE is_delete = 0;";
            var DataList = new List<Carousel>();

            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    Carousel Data = new Carousel();
                    Data.carousel_image_id = (Guid)dr["carousel_image_id"];
                    if (dr["carousel_image"] != DBNull.Value)
                    {
                        var imagesStr = dr["carousel_image"].ToString();
                        var hosturl = "http://localhost:5229/";
                        if (!string.IsNullOrEmpty(imagesStr))
                        {
                            var images = imagesStr.Split(',').ToList();

                            for (int i = 0; i < images.Count; i++)
                            {
                                images[i] = hosturl+$"Image/{images[i]}"; 
                            }
                            Data.carousel_image = images;
                        }
                        else
                        {
                            Data.carousel_image = null;
                        }
                    }
                    else
                    {
                        Data.carousel_image = null;
                    }
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

        public void InsertCarousel(Carousel newData)
        {
            string sql = $@"INSERT INTO Carousel
                            (carousel_image_id,carousel_image,create_time,create_id,
                            update_time,update_id,is_delete) 
                            VALUES
                            (@carousel_image_id, @carousel_image,
                            @create_time, @create_id, @update_time, @update_id,0);";
            
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.carousel_image_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@carousel_image_id", newData.carousel_image_id);
                cmd.Parameters.AddWithValue("@carousel_image", newData.image);
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

        public Carousel GetDataById(Guid Id)
        {
            string sql = $@"SELECT * FROM Carousel WHERE carousel_image_id = @Id AND is_delete=0;";

            Carousel Data = new Carousel();

            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.carousel_image_id = (Guid)dr["carousel_image_id"];
                if (dr["carousel_image"] != DBNull.Value)
                    {
                        var imagesStr = dr["carousel_image"].ToString();
                        var hosturl = "http://localhost:5229/";
                        if (!string.IsNullOrEmpty(imagesStr))
                        {
                            var images = imagesStr.Split(',').ToList();

                            for (int i = 0; i < images.Count; i++)
                            {
                                images[i] = hosturl+$"Image/{images[i]}"; 
                            }
                            Data.carousel_image = images;
                        }
                        else
                        {
                            Data.carousel_image = null;
                        }
                    }
                else
                {
                    Data.carousel_image = null;
                }
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

        public void UpdateCarousel(Carousel updateData)
        {
            string sql = $@"UPDATE Carousel 
                            SET 
                            carousel_image = @carousel_image,update_time = @update_time,update_id = @update_id 
                            WHERE 
                            carousel_image_id = @Id;";
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.carousel_image_id);
                cmd.Parameters.AddWithValue("@update_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@update_id", updateData.update_id);
                string imagesString = string.Join(",", updateData.carousel_image);
                cmd.Parameters.AddWithValue("@carousel_image", imagesString);
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

        public void SoftDeleteCarouselById(Guid id)
        {
            string sql = $@"UPDATE Carousel SET is_delete = 1 WHERE carousel_image_id = @Id;";

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