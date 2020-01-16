using System;
using System.DirectoryServices;
namespace LxGreg.services
{
    public class AdHelper
    {
        private DirectoryEntry domain;
        DirectorySearcher searcher1;
        #region 构造函数及初始化
        /// <summary>
        /// 可以管理用户的构造函数
        /// </summary>
        /// <param name="usname"></param>
        /// <param name="pw"></param>
        public AdHelper(string usname,string pw)
        {
            Initial();
            domain.Username = usname;
            domain.Password = pw;
        }
        /// <summary>
        /// 仅用于查询的构造函数
        /// </summary>
        public AdHelper()
        {
            Initial();
        }
        private void Initial()
        {
            domain = new DirectoryEntry();
            //domain.Path = "LDAP://ad4-sh/ou=力信,dc=lixin,dc=com";
            domain.Path = "LDAP://192.168.1.53/dc=ysq,dc=com";
            searcher1 = new DirectorySearcher(domain);
            searcher1.SearchScope = SearchScope.Subtree;
            searcher1.SearchRoot = domain;
        }
        #endregion
        #region 转型\判断存在\精确查找de对象
        /// <summary>
        /// 获取单个用户De对象
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DirectoryEntry getuser(string userid)
        {
            searcher1.Filter = $"(&(objectClass=user)(sAMAccountname={userid}))";
            var de = searcher1.FindOne();
            if (de == null) { return null; }
            return searcher1.FindOne().GetDirectoryEntry();
        }
        /// <summary>
        /// 判断账号是否存在
        /// </summary>
        /// <param name="name">Id</param>
        /// <returns></returns>
        /// <summary>
        /// 精确获得单个用户de
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        #endregion
        #region 高级方法,需要鉴权
        /// <summary>
        /// 配置用户属性的方法
        /// </summary>
        /// <param name="de">用户de</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="PropertyValue">属性值</param>
        public void SetProperty(DirectoryEntry de, string PropertyName, string PropertyValue)
        {
            try
            {
                if (de == null)
                {
                    throw new Exception("用户未找到！");
                }
                if (PropertyValue != null)
                {
                    if (de.Properties.Contains(PropertyName))
                    {
                        de.Properties[PropertyName][0] = PropertyValue;
                    }
                    else
                    {
                        de.Properties[PropertyName].Add(PropertyValue);
                    }
                    de.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"底层错误：{ex.Message}");
            }

        }
        /// <summary>
        /// 启用账号
        /// </summary>
        /// <param name="de">用户的DirectoryEntry对象</param>
        /// <returns></returns>
        public string Enable(string id)
        {
            var de = getuser(id);
            if (de == null)
            {
                throw new Exception("用户未找到！");
            }
            de.Properties["userAccountControl"].Value = 0x0200 | 0x10000;
            de.CommitChanges();
            return "ok";
        }
        /// <summary>
        /// 禁用账号
        /// </summary>
        /// <param name="de">用户的DirectoryEntry对象</param>
        /// <returns></returns>
        public string Disable(string id)
        {
            var de = getuser(id);
            if (de == null)
            {
                throw new Exception("用户未找到！");
            }
            de.Properties["userAccountControl"].Value = 0x0200 | 0x10000 | 0x0002;
            de.CommitChanges();
            return "ok";
        }
        /// <summary>
        /// 重置域密码
        /// </summary>
        /// <param name="sysid">域账号</param>
        /// <param name="newpwd">新密码</param>
        /// <returns></returns>
        public string resetpassword(string sysid,string pd,string newpwd)
        {
            try
            {
                var test= verify(sysid, pd);
                if (!test)
                {
                    return "密码错误，清重新输入，初始密码为Qaz123";
                }
                //   domain.Password = pd;
                domain.Username = "radmin";
                domain.Password = "rer0y%";
                DirectorySearcher searcher1 = new DirectorySearcher(domain);
                searcher1.Filter = "(&(objectClass=user)(sAMAccountname=" + sysid + @"))";
                searcher1.SearchScope = SearchScope.Subtree;
                searcher1.SearchRoot = domain;
                SearchResultCollection rs = searcher1.FindAll();
                foreach (SearchResult item in rs)
                {
                    DirectoryEntry obj = item.GetDirectoryEntry();
                    //  Console.WriteLine(obj.Name);
                    if (rs.Count > 0)
                    {
                        // Console.WriteLine("已存在账号"+obj.Path);//如果有了就输出一下
                        object ret = obj.Invoke("SetPassword", newpwd);
                        obj.Properties["userAccountControl"].Value = 0x0200 | 0x10000;
                        obj.CommitChanges();
                        return "ok";
                    }
                    else
                    {
                        return "找不到此用户";
                    }

                }
                return null;

            }
            catch (Exception ex)
            {
                return sysid + "///" + ex.Message;

            }
        }







        /// <summary>
        /// 查询所在部门组织架构的路径
        /// </summary>
        /// <param name="depart">部门</param>
        /// <returns></returns>
        public DirectoryEntry queryou(string depart)
        {
           
            string part = "";
            try
            {
                DirectorySearcher searcher1 = new DirectorySearcher(domain);
                searcher1.Filter = "(name=" + depart + ")";
                searcher1.SearchScope = SearchScope.Subtree;
                searcher1.SearchRoot = domain;
                SearchResultCollection rs = searcher1.FindAll();
                foreach (SearchResult item in rs)
                {
                    DirectoryEntry obj = item.GetDirectoryEntry();
                    if (obj.SchemaClassName == "organizationalUnit")//判断是否为组织架构
                    {
                        return obj;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;

        }
        /// <summary>
        /// 判断账号是否存在
        /// </summary>
        /// <param name="name">姓名</param>
        /// <returns></returns>
        public DirectoryEntry isex(string name)
        {
            DirectorySearcher searcher1 = new DirectorySearcher(domain);
            searcher1.Filter = "(&(objectClass=user)(name=" + name + @"))";
            searcher1.SearchScope = SearchScope.Subtree;
            searcher1.SearchRoot = domain;
            SearchResultCollection rs = searcher1.FindAll();
            foreach (SearchResult item in rs)
            {
                DirectoryEntry obj = item.GetDirectoryEntry();
                //  Console.WriteLine(obj.Name);
                return obj;
                if (rs.Count > 0)
                {
                    // Console.WriteLine("已存在账号"+obj.Path);//如果有了就输出一下
                    //   return true;
                }
            }
            return null;

            //   return false;
        }
        public DirectoryEntry querygroup(string depart)
        {
            try
            {
                DirectorySearcher searcher1 = new DirectorySearcher(domain);
                searcher1.Filter = "(name=" + depart + ")";
                searcher1.SearchScope = SearchScope.Subtree;
                searcher1.SearchRoot = domain;
                SearchResultCollection rs = searcher1.FindAll();
                foreach (SearchResult item in rs)
                {
                    DirectoryEntry obj = item.GetDirectoryEntry();

                    if (obj.SchemaClassName == "group")
                    {
                        return obj;
                    }


                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public bool verify(string userId,string userPwd)
        {
            try
            {
                DirectoryEntry entry = new DirectoryEntry(domain.Path, userId, userPwd);
                DirectorySearcher search = new DirectorySearcher(entry); //创建DirectoryEntry对象的搜索对象
                search.Filter = "(SAMAccountName=" + userId + ")";  //过滤条件为登录帐号＝user
                SearchResult result = search.FindOne(); //查找第一个
                if (null == result)   //没找到
                {
                   // retmsg = "cancel";
                    return false;
                    
                }
                return true;
            }
            catch (DirectoryServicesCOMException ex)
            {
                if (ex.ErrorCode == -2147023570)
                {
                    return false;
                }
                return false;

            }

        }
        #endregion




        public string CreateGroup(string name, string parentou)
        {
          
            var check = querygroup(name);

            if (check == null)
            {
                var parent = queryou(name);

                check = querygroup(name);
                if (check == null)
                {
                    DirectoryEntry target = parent.Children.Add("CN=" + name, "group");
                    target.CommitChanges();
                    parent = querygroup(parentou);
                    parent.Properties["member"].Add(target.Properties["distinguishedName"].Value);

                    parent.CommitChanges();
                }
            }
            else
            {
                return "组已经存在";
            }
            return "ok";
        }
        public string CreateOU(string name, string parentou)
        {
          
            var check = queryou(name);
            if (check == null)
            {
                var parent = queryou(parentou);
                DirectoryEntry target = parent.Children.Add("OU=" + name, "organizationalUnit");
                target.CommitChanges();
                parent.CommitChanges();
            }
            else
            {
                return "组织架构已经存在";
            }

            return "ok";
        }

        /// <summary>
        /// 创建用户
        /// <param name="name">姓名</param>
        /// <param name="part"></param>
        public string CreateNewUser(string login, string name, string part, string employeeID, string email)
        {
           
            string result;
            var checkuser = isex(name);
            if (checkuser != null)
            {
                //  string checkeduser = $"用户已经存在，账户{checkuser.Name}，父部门{checkuser.Parent.Name}";
                return checkuser.Parent.Name.Substring(3);

            }

            try
            {

                string path = queryou(part).Path;

                domain.Path = path;


                DirectoryEntries users = domain.Children;
                DirectoryEntry newuser = users.Add("CN=" + name, "user"); //目录里显示的名称
                string lastname = name.Substring(0, 1);//拆姓
                string firstname = name.Substring(1);//拆名
                SetProperty(newuser, "givenname", firstname);
                SetProperty(newuser, "sn", lastname);
                SetProperty(newuser, "displayname", name);
                SetProperty(newuser, "userPrincipalName", login + @"@ysq.com");
                SetProperty(newuser, "SAMAccountName", login);
                SetProperty(newuser, "Description", "Create User from CSharp program design by Greg Liu");
                SetProperty(newuser, "mail", email);
                SetProperty(newuser, "employeeNumber", employeeID);

                try
                {
                    newuser.CommitChanges();
                    Enable(login);
                    newuser.AuthenticationType = AuthenticationTypes.Secure;
                    object ret = newuser.Invoke("SetPassword", "Qaz123");
                    newuser.CommitChanges();
                    domain.Path = querygroup(part).Path;
                    domain.Properties["member"].Add(newuser.Properties["distinguishedName"].Value);
                    domain.CommitChanges();
                    domain.Close();
                    newuser.Close();



                }
                catch (Exception ex)
                {
                    throw ex;
                }



                result = "ok";
                return result;

            }
            catch (Exception ex)
            {
                result = ex.Message + name;
                return result;
            }



            // AddUserToGroup(newuser, group);

        }



    }
}
