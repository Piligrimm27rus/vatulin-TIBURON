using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace vatulin_TIBURON.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewController : ControllerBase
    {
        string sql = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Testovoe-vatulin-tiburon;Integrated Security=True;";

        [HttpGet("{id}")]
        public string Get(int id)
        {
            string query = "select Question.Question, Answer.Ans1, Answer.Ans2, Answer.Ans3 from Question, Answer " +
                           $"where Question.Id = {id} and Question.AnswerId = Answer.Id";

            string getResult = "";

            using (SqlConnection con = new SqlConnection(sql))
            {
                con.Open();

                getResult = DoCommand(query, con);
            }

            return getResult;
        }

        //{
        //    "QuestionId": 4,
        //    "Answer": "Answer",
        //    "UserId": 0
        //}
        [HttpPost]
        public string Post([FromBody] UserResult ur)
        {
            string query = $"insert into Result (QuestionId, Answer, UserId) values ({ur.QuestionId}, \'{ur.Answer}\', {ur.UserId})";
            string query1 = $"select top(1) Id from Question where Id > {ur.QuestionId} " +
                             $"and ServeyId = (select ServeyId from Question where Id = {ur.QuestionId})";

            string postResult = "";

            using (SqlConnection con = new SqlConnection(sql))
            {
                con.Open();

                string commandResult = DoCommand(query, con);
                postResult = DoCommand(query1, con);
            }

            return postResult;
        }

        private string DoCommand(string query, SqlConnection con)
        {
            DataTable table = new DataTable();
            SqlDataReader dataReader;

            using (SqlCommand myCommand = new SqlCommand(query, con))
            {
                dataReader = myCommand.ExecuteReader();
                table.Load(dataReader);

                dataReader.Close();
            }

            return JsonConvert.SerializeObject(table);
        }
    }
}
