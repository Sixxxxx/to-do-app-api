
using ToDoList.Services.Interfaces;

namespace ToDoList.Services.Implementation
{
    public class CircleService : ICircleService
    {
        public double CalculateArea(double radius)
        {
            return Math.PI * radius * radius;
        }
        
    }
}
