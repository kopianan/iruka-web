namespace Iruka.ModelAPI
{
    public class GetUserByRoleModelRequest
    {
        public string Role { get; set; }

        public GetUserByRoleModelRequest(string role)
        {
            Role = role;
        }
    }
}