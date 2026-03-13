namespace QuantityMeasurementConsoleApp.Interface
{
    public interface IMenu
    {
        void Start();
        void DisplayMainMenu();
        void DisplayMeasurementTypeMenu();
        void DisplayOperationMenu();
        int GetUserChoice();
        void DisplayResult(string result);
        void DisplayError(string error);
        void WaitForUserInput();
        void ClearScreen();
    }
}