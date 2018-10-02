namespace Utils
{
    public interface IFitbitConfig
    {   

        string FitbitClientId { get; }
        string FitbitClientSecret { get; }

        string FitbitUserId { get; }

        string FitbitVerificationCode { get; }
    }
}