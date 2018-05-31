using System;

namespace AnonyIsland.Tools
{
    static class CommentTool
    {
        static readonly string _receiveHtml = @"<script type=""text/javascript"">window.location.hash = ""#ok"";</script> 
            <div class=""chat_content_group buddy"">   
            <img class=""chat_content_avatar"" onclick=""window.external.notify('{4}-{5}')"" style=""cursor:pointer"" src=""{0}"" width=""40px"" height=""40px""/> 
            <p class=""chat_nick"" style=""cursor:pointer;font-family:微软雅黑"" onclick=""window.external.notify('{4}-{5}')"">{1}</p>   
            <p class=""chat_time"">{2}</p>            
            <p class=""chat_content"">{3}</p>
            </div>";
        
        public static string BaseChatHtml { get; } = @"<html><head>
        <script type=""text/javascript"">window.location.hash = ""#ok"";</script>
        <style type=""text/css"">
        body{
        font-family:微软雅黑;
        font-size:14px;
        border-top: #999 solid 2px;
        }
      
        textarea{width: 500px;height: 300px;border: none;padding: 5px;}  

        .chat_content_group.self {
        text-align: right;
        }
        .chat_content_group {
        padding: 5px;
        }
        .chat_content_group.self>.chat_content {
        text-align: left;
        }
        .chat_content_group.self>.chat_content {
        background: #7ccb6b;
        color:#fff;
        }
        .chat_content {
        clear: both;
        margin: 0;
        display: block;
        min-height: 16px;
        max-width: 100%;
        color:#292929;
        font-family:微软雅黑;
        font-size:14px;
        padding-top: 10px;
        border-radius: 5px;
        word-break: break-all;
        line-height: 1.4;
        }

        .chat_content_group.self>.chat_nick {
        text-align: right;
        }
        .chat_nick {
        font-size: 14px;
        margin: 0 0 5px;
        color:#8b8b8b;
        }

        .chat_content_group.self>.chat_content_avatar {
        float: right;
        margin: 0 0 0 10px;
        }

        .chat_content_group.buddy {
        text-align: left;
        }
        .chat_content_group {
        padding: 10px;
        }
        .chat_content_avatar {
        float: left;
        width: 40px;
        height: 40px;
        margin-right: 10px;
        }
        .imgtest{margin:10px 5px;
        overflow:hidden;}
        .list_ul figcaption p{
        font-size:11px;
        color:#aaa;
        }
        .imgtest figure div{
        display:inline-block;
        margin:5px auto;
        width:100px;
        height:100px;
        border-radius:100px;
        border:2px solid #fff;
        overflow:hidden;
        box-shadow:0 0 3px #ccc;
        }
        .imgtest img{width:100%;
        min-height:100%; text-align:center;}

        .chat_time{
            margin: 0;
            color:#8b8b8b;
            font-size: 12px;
        }
	    </style>
        </head><body>";

        public static string Receive(string avator, string nickname, string content, string time, string commentId = null)
        {
            string id = commentId ?? "";
            return String.Format(_receiveHtml, avator, nickname, time, content, id, nickname);
        }
    }
}
