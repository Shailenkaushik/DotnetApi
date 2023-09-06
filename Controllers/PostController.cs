using DotnetAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Models;
namespace Controllers{

    // [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class PostController :ControllerBase {

        private readonly DataContextDapper _dapper;
        public PostController(IConfiguration config){
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("Posts")]
        public IEnumerable<Post> GetPosts(){
                string sql=@"Select [PostId],
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated] from TutorialAppSchema.Posts";

            return _dapper.LoadData<Post>(sql);
        }

        [HttpGet("PostSingle/{postId}")]
        public IEnumerable<Post> GetSinglePosts(int postId){
                string sql=@"Select [PostId],
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated] from TutorialAppSchema.Posts
                where PostId="+postId.ToString();
                
            return _dapper.LoadData<Post>(sql);
        }

        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts(){
                string sql=@"Select [PostId],
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
               [PostUpdated] from TutorialAppSchema.Posts
                where PostId="+this.User.FindFirst("userId")?.Value;
            return _dapper.LoadData<Post>(sql);
        }


        [HttpGet("PostsBySearch/{searchParam}")]
        public IEnumerable<Post> GetMyPosts(string searchParam ){
                string sql=@"Select [PostId],
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
               [PostUpdated] from TutorialAppSchema.Posts
                where PostTitle LIKE '%" +searchParam +"%'"+
                   " OR postContent LIKE '%" +searchParam +"%'";
            return _dapper.LoadData<Post>(sql);
        }



        [HttpPost("Post")]

        public IActionResult AddPost(PostToAddDto postToAdd){
           
           string sql=@"Insert into TutorialAppSchema.Posts(
                        [UserId],
                        [PostTitle],
                        [PostContent],
                        [PostCreated],
                        [PostUpdated])
                        values("+this.User.FindFirst("userId")?.Value
                         +",' "+ postToAdd.PostTitle
                         +" ', ' " + postToAdd.PostContent
                         +" ', GetDate(),GetDate() )";
           
             if(_dapper.ExecuteSql(sql)){
                return Ok();
             }
           throw new Exception("Gailed to add post");
        }


         [HttpPut("Post")]

        public IActionResult EditPost(PostToEditDto postToEdit){
           
           string sql=@"
           Update  TutorialAppSchema.Posts
                       SET  PostTitle=' "+postToEdit.PostTitle + 
                       " ', PostContent='"+ postToEdit.PostContent
                        + @"',PostUpdated=Getdate()
                            where PostId="+ postToEdit.PostId.ToString()+
                            "AND UserId="+ this.User.FindFirst("userId")?.Value;

                       
           
             if(_dapper.ExecuteSql(sql)){
                return Ok();
             }
           throw new Exception("Gailed to edit post");
        }
         
           [HttpDelete("Post/{PostId}")]

           public IActionResult DeletePost(int PostId){

            string sql=@"Delete from TutorialAppSchema.Posts
             where PostId="+PostId.ToString();
            if(_dapper.ExecuteSql(sql)){
                return Ok();
            }
            throw new Exception("Gailed to delete user");
           }

    }
}