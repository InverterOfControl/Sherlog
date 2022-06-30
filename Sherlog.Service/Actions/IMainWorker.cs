namespace Sherlog.Service.Actions
{
  public interface IMainWorker
  {
    void DoWork(CancellationToken cancellationToken);
  }
}
