using System.Threading.Tasks;

namespace AGL.CodeExercise.App
{
    public interface IPetsController
    {
        Task GetCatsPerOwnerGender();
    }
}
