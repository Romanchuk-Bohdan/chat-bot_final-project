namespace ChatBot.Core.Interfaces
{
    public interface ICommandStrategy
    {
        string Execute(string[] args, string currentUserId);
    }
}