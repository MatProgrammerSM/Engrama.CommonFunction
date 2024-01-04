using System.Threading.Tasks;

namespace CommonFuncion.Identity
{
	public interface ILoginService
	{
		Task LogIn(string token);
		
		Task LogOut();
	}
}