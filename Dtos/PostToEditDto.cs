namespace Controllers{

    public class PostToEditDto{

           public int PostId  {get; set;}
            public  int UserId {get; set;}
            public string PostTitle  {get; set;}
            public string PostContent  {get; set;}
            public DateTime PostCreated  {get; set;}
            public DateTime PostUpdated  {get; set;}
              
              public PostToEditDto(){
                if(PostTitle==null) PostTitle="";
              if(PostContent==null) PostContent="";
              }
    }
}