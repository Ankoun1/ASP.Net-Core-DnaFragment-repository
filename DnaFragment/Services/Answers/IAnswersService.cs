namespace DnaFragment.Services.Answers
{
    public interface IAnswersService
    {
        void AddAnswerDb(string questId,string description);

        void DeleteAnswerDb(string answerId);
    }
}
