using ToDoList.Services.Interfaces;

namespace ToDoList.Services.Implementation
{
    public class TriangleService : ITriangleService
    {
  
        // provide implementation for CalculateArea()
        public double CalculateArea(double basee, double height)
        {
            return 0.5 * basee * height;
        }
    }
}
