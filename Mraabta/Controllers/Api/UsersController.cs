using MRaabta.Models.Api;
using MRaabta.Repo.Api;
using MRaabtaApis.Models.Api;
using System;
using System.Configuration;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace MRaabta.Controllers.Api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {

        AccountDB accdb_ = new AccountDB();
        DateTime issuedAt = DateTime.Now;
        DateTime expires = DateTime.Now.Date.Add(new TimeSpan(23, 59, 59));

        [HttpGet]
        public userDetailResponse Authenticate(string riderCode, string password, string imei1, string imei2, string simSno)
        {
            var userDetailResponse_ = new userDetailResponse();
            try
            {

                //int userID, createdBy, RoleId;
                //DateTime createdOn = new DateTime();
                String message = "";
                //string RiderCode, branchCode, username, message = "";
                var loginResponse = new userDetailResponse();
                IHttpActionResult response;
                // HttpResponseMessage responseMsg = new HttpResponseMessage();
                bool isUsernamePasswordValidRider = false; bool isSuccess = true;

                Accounts acc_ = new Accounts();

                if (riderCode == null || password == null)
                {
                    message = "Kindly send both username and password, empty string not allowed.";
                    userDetailResponse_ = new userDetailResponse
                    {
                        isSuccess = false,
                        message = message,
                        accountObj = null,
                        token = "null",

                    };
                    return userDetailResponse_;
                }

                DataTable dt_ = accdb_.validateUser(riderCode, password, imei1, imei2, simSno);

                if (dt_.Rows.Count > 0)
                {
                    acc_ = new Accounts
                    {
                        userID = Convert.ToInt32(dt_.Rows[0]["USER_ID"]),
                        branchCode = dt_.Rows[0]["branchCode"].ToString(),
                        username = dt_.Rows[0]["userName"].ToString(),
                        RiderCode = dt_.Rows[0]["riderCode"].ToString(),
                        RoleId = Convert.ToInt32(dt_.Rows[0]["roleID"]),
                        Status = 1,
                        ///////////////////////////
                    };
                    userDetailResponse_ = new userDetailResponse
                    {
                        isSuccess = true,
                        message = "Success",
                        accountObj = acc_,
                        token = createTokenForRider(riderCode),
                    };
                    bool result = accdb_.AttendanceIn(Convert.ToInt32(dt_.Rows[0]["USER_ID"]));
                    if (!result)
                    {
                        isSuccess = false;
                        message = "Correct credentials, but error in logging attendance";
                        userDetailResponse_ = new userDetailResponse
                        {
                            isSuccess = isSuccess,
                            message = message,
                            accountObj = null,
                            token = createTokenForRider(riderCode),
                        };
                    }
                }
                else
                {
                    isSuccess = false;
                    message = "invalid credentials";
                    userDetailResponse_ = new userDetailResponse
                    {
                        isSuccess = isSuccess,
                        message = message,
                        accountObj = null,
                        token = "null",
                    };

                    //// if credentials are not valid send unauthorized status code in response
                    //loginResponse.responseMsg.StatusCode = HttpStatusCode.Unauthorized;
                    //response = ResponseMessage(loginResponse.responseMsg);
                    //return response;
                }
                return userDetailResponse_;
            }
            catch (Exception er)
            {
                userDetailResponse_ = new userDetailResponse
                {
                    isSuccess = false,
                    message = "Error",
                    accountObj = null,
                    token = null,
                };
                return userDetailResponse_;
            }
        }

        private string createTokenForRider(string username)
        {

            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role,"rider")
            });

            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));

            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);


            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: "http://localhost:2467", audience: "http://localhost:2467",
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        //[ResponseType(typeof(userDetailResponse))]
        [HttpGet, ActionName("Login")]
        public userDetailResponse Login(string riderCode, string password, string imei1 = null, string imei2 = null, string simSno = null)
        {
            int userID, createdBy, RoleId;
            DateTime createdOn = new DateTime();
            string RiderCode, branchCode, username, message = "";
            bool isSuccess = false;
            var userDetailResponse_ = new userDetailResponse();
            try
            {
                Accounts acc_ = new Accounts();
                if (riderCode == null || password == null)
                {
                    userDetailResponse_ = new userDetailResponse
                    {
                        isSuccess = false,
                        message = "Kindly send both username and password, empty string not allowed.",
                        accountObj = null,
                        Version = ConfigurationManager.AppSettings["deliveryappversion"]
                    };

                    return userDetailResponse_;
                }

                DataTable dt_ = accdb_.validateUser(riderCode, password, imei1, imei2, simSno);

                if (dt_.Rows.Count > 0)
                {
                    acc_ = new Accounts
                    {
                        userID = Convert.ToInt32(dt_.Rows[0]["USER_ID"]),
                        branchCode = dt_.Rows[0]["branchCode"].ToString(),
                        username = dt_.Rows[0]["userName"].ToString(),
                        RiderCode = dt_.Rows[0]["riderCode"].ToString(),
                        RoleId = Convert.ToInt32(dt_.Rows[0]["roleID"]),
                        Status = 1
                        ///////////////////////////
                    };
                    userDetailResponse_ = new userDetailResponse
                    {
                        isSuccess = true,
                        message = "Success",
                        accountObj = acc_,
                        Version = ConfigurationManager.AppSettings["deliveryappversion"]
                    };

                    bool result = accdb_.AttendanceIn(Convert.ToInt32(dt_.Rows[0]["USER_ID"]));
                    bool result1 = accdb_.IsActiveIn(Convert.ToInt32(dt_.Rows[0]["USER_ID"]));
                    if (!result)
                    {
                        isSuccess = false;
                        message = "Correct credentials, but error in logging attendance";
                        userDetailResponse_ = new userDetailResponse
                        {
                            isSuccess = isSuccess,
                            message = message,
                            accountObj = null,
                            Version = ConfigurationManager.AppSettings["deliveryappversion"]
                        };
                    }
                    if (!result1)
                    {
                        isSuccess = false;
                        message = "Already LoggedIn";
                        userDetailResponse_ = new userDetailResponse
                        {
                            isSuccess = isSuccess,
                            message = message,
                            accountObj = null,
                            Version = ConfigurationManager.AppSettings["deliveryappversion"]
                        };
                    }
                }
                else
                {
                    isSuccess = false;
                    message = "invalid credentials";
                    userDetailResponse_ = new userDetailResponse
                    {
                        isSuccess = isSuccess,
                        message = message,
                        accountObj = null,
                        Version = ConfigurationManager.AppSettings["deliveryappversion"]
                    };
                }
                return userDetailResponse_;
            }
            catch (Exception ee)
            {
                userDetailResponse_ = new userDetailResponse
                {
                    isSuccess = false,
                    message = "Error Occured "+ ee.Message,
                    accountObj = null,
                    Version = ConfigurationManager.AppSettings["deliveryappversion"]
                };

                return userDetailResponse_;
            }
        }

        [ResponseType(typeof(userDetailLogout))]
        [HttpGet]
        // [Authorize]
        public IHttpActionResult Logout(String riderCode = null)
        {


            var userDetailResponse_ = new userDetailLogout();

            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                String RiderCodeFromToken = identity.Claims.FirstOrDefault().Value;
            }

            string message = "";
            bool isSuccess = false, is_loggedIn = false;
            try
            {
                Accounts acc_ = new Accounts();
                if (riderCode == null)
                {
                    isSuccess = false;
                    message = "no username/riderCode provided";
                    userDetailResponse_ = new userDetailLogout
                    {
                        isSuccess = isSuccess,
                        message = message,
                        logout = "error can't logout , no rider to logout ",
                        Version = ConfigurationManager.AppSettings["deliveryappversion"]
                    };

                    return Ok(userDetailResponse_);
                }


                DataTable dt_ = accdb_.validateUserLogout(riderCode);
                if (dt_.Rows.Count > 0)
                {

                    Tuple<bool, string> result = accdb_.AttendanceOut(int.Parse(dt_.Rows[0]["USER_ID"].ToString()));
                    Tuple<bool, string> result1 = accdb_.UpdateStatus(int.Parse(dt_.Rows[0]["USER_ID"].ToString()));


                    userDetailResponse_ = new userDetailLogout
                    {
                        isSuccess = true,
                        message = "Success",
                        logout = "logged out successfully",
                        Version = ConfigurationManager.AppSettings["deliveryappversion"]
                    };
                }
                else
                {
                    userDetailResponse_ = new userDetailLogout
                    {
                        isSuccess = false,
                        message = "No account found against this UserId",
                        logout = "Error, not account found",
                        Version = ConfigurationManager.AppSettings["deliveryappversion"]
                    };

                }
                return Ok(userDetailResponse_);
            }
            catch (Exception ee)
            {
                userDetailResponse_ = new userDetailLogout
                {
                    isSuccess = false,
                    message = "Error Occured",
                    logout = "Error!",
                    Version = ConfigurationManager.AppSettings["deliveryappversion"]
                };

                return Ok(userDetailResponse_);
            }

        }

        [ResponseType(typeof(userDetailLogout))]
        [HttpGet]
        public IHttpActionResult Absent(String riderCode = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as ClaimsIdentity;
                String RiderCodeFromToken = identity.Claims.FirstOrDefault().Value;
            }

            string message = "";
            bool isSuccess = true;
            var userDetailResponse_ = new userDetailLogout();
            try
            {
                Accounts acc_ = new Accounts();
                if (riderCode == null)
                {
                    isSuccess = false;
                    message = "no username/riderCode provided";
                    userDetailResponse_ = new userDetailLogout
                    {
                        isSuccess = isSuccess,
                        message = message,
                        logout = "error can't logout , no rider to logout "
                    };

                    return Ok(userDetailResponse_);
                }
                DataTable dt_ = accdb_.validateUserLogout(riderCode);

                if (dt_.Rows.Count > 0)
                {


                    Tuple<bool, string> result = accdb_.AbsentMarkAttendance(int.Parse(dt_.Rows[0]["USER_ID"].ToString()));


                    if (result.Item1)
                    {
                        userDetailResponse_ = new userDetailLogout
                        {
                            isSuccess = true,
                            message = "Success",
                            logout = "Absent Marked Successfully"
                        };

                    }
                    else
                    {
                        userDetailResponse_ = new userDetailLogout
                        {
                            isSuccess = false,
                            message = "error in logging attendance out",
                            logout = "absent entry error"
                        };
                    }
                }
                else
                {
                    isSuccess = false;
                    message = "invalid credentials";
                    userDetailResponse_ = new userDetailLogout
                    {
                        isSuccess = false,
                        message = message,
                        logout = "Can't find a rider againt this riderCode"
                    };
                }
                return Ok(userDetailResponse_);
            }
            catch (Exception ee)
            {
                userDetailResponse_ = new userDetailLogout
                {
                    isSuccess = false,
                    message = "Error Something went wrong",
                    logout = "Error cannot logout."
                };

                return Ok(userDetailResponse_);
            }

        }

        [HttpPost, ActionName("ChangePassword")]
        public async Task<HttpResponseMessage> ChangePassword([FromBody] AppUserModel model)
        {
            try
            {
                var rs = await accdb_.ChangePass(model.RiderCode, model.Pass, model.NewPass);
                return rs ? Request.CreateResponse(HttpStatusCode.OK, new { status = "success", message = "password changed" }) : Request.CreateResponse(HttpStatusCode.ExpectationFailed, new { status = "error", message = "something went wrong" });
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet, ActionName("GetAdminPasswords")]
        public async Task<HttpResponseMessage> GetAdminPasswords(string branchCode)
        {
            try
            {
                var rs = await accdb_.GetAdminPasswords(branchCode);
                return Request.CreateResponse(HttpStatusCode.OK, new { rs.AdminPass1, rs.AdminPass2 });
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet, ActionName("GetVersion")]
        public HttpResponseMessage GetVersion()
        {
            var version = ConfigurationManager.AppSettings["deliveryappversion"].ToString();
            return Request.CreateResponse(HttpStatusCode.OK, new { version });
        }

        [HttpGet, ActionName("GetReasons")]
        public async Task<HttpResponseMessage> GetReasons()
        {
            try
            {
                var rs = await accdb_.GetReasons();
                return Request.CreateResponse(HttpStatusCode.OK, new { reasons = rs });
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet, ActionName("GetRelations")]
        public async Task<HttpResponseMessage> GetRelations()
        {
            try
            {
                var rs = await accdb_.GetRelations();
                return Request.CreateResponse(HttpStatusCode.OK, new { relations = rs });
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
