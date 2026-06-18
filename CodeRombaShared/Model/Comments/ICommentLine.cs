namespace WinterbiteStudios.CodeRomba.Model.Comments
{
    internal interface ICommentLine
    {
        string Content { get; }

        bool IsLast { get; }
    }
}