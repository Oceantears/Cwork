using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;
using GameServer.DAO;
using GameServer.Model;

namespace GameServer.Controller
{
    class UserController:BaseController
    {
        private UserDAO userDao=new UserDAO();
        public UserController()
        {
            requestCode = RequestCode.User;
        }

        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            User user = userDao.VerifyUser(client.MySqlConn, strs[0], strs[1]);
            if (user == null)
            {
                //Enum.GetName(typeof(ReturnCode), ReturnCode.Fail);
                return ((int) ReturnCode.Fail).ToString();            //两种方法返回，后者占用空间小，所以用后者
            }
            else
            {
                return ((int)ReturnCode.Success).ToString();
            }
        }
    }
}
