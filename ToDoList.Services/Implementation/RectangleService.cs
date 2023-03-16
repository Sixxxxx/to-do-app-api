
using ToDoList.Services.Interfaces;

namespace ToDoList.Services.Implementation
{
    public class RectangleService : IRectangleService
    {
        public double CalculateArea(double basee, double height)
        {
            return basee * height;
        }
    }
}
