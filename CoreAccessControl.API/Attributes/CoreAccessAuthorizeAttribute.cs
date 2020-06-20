using CoreAccessControl.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Supra.LittleLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAccessControl.API.Attributes
{
    public class CoreAccessAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly PermissionDomain _domain;
        private readonly PermissionAction[] _actions;
        private readonly PermissionActionCondition _condition;

        public CoreAccessAuthorizeAttribute(PermissionDomain domain, PermissionAction action)
        {
            this._domain = domain;
            this._actions = new PermissionAction[] { action };
            this._condition = PermissionActionCondition.And;
        }

        public CoreAccessAuthorizeAttribute(PermissionDomain domain, PermissionActionCondition condition, params PermissionAction[] actions)
        {
            this._domain = domain;
            this._actions = actions;
            this._condition = condition;
        }

        private JWTPayload GetJWTPayload(AuthorizationFilterContext context)
        {
            // handle the request  
            return JsonConvert.DeserializeObject<JWTPayload>(context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == Constants.JWTPayloadClaim).Value);
        }

        private Permission GetPermission(long locationId, AuthorizationFilterContext context)
        {
            return GetJWTPayload(context).Permissions.FirstOrDefault(x => x.LocationId == locationId);
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.RouteValues.ContainsKey("locationId"))
            {
                Logger.WriteInformation("Forbidden request for missing locationid parameter");
                var result = new ObjectResult(new { Message = "You do not have enough permission to execute the operation" });
                result.StatusCode = 403;
                context.Result = result;
                return;
            }

            var locationId = long.Parse(context.HttpContext.Request.RouteValues["locationId"].ToString());
            var permission = GetPermission(locationId, context);

            var hasPermission = HasPermission(context, permission);
            if (!hasPermission)
            {
                Logger.WriteInformation("Forbidden request for not having proper permission", Severity.Warning);
                var result = new ObjectResult(new { Message = "You do not have enough permission to execute the operation" });
                result.StatusCode = 403;
                context.Result = result;
            }
        }

        private bool HasPermission(AuthorizationFilterContext context, Permission permission)
        {
            Logger.WriteInformation("Calculating permission");
            if (permission == null)
            {
                return false;
            }

            switch (_domain)
            {
                case PermissionDomain.Admin:
                    {
                        var action = _actions[0];
                        var result = false;
                        if (action == PermissionAction.Write)
                        {
                            result = permission.HasAdminEdit;
                        }
                        if (action == PermissionAction.Read)
                        {
                            result = permission.HasAdminRead;
                        }

                        for (int i = 1; i < _actions.Length; i++)
                        {
                            action = _actions[i];
                            var result1 = false;
                            if (action == PermissionAction.Write)
                            {
                                result1 = permission.HasAdminEdit;
                            }
                            if (action == PermissionAction.Read)
                            {
                                result1 = permission.HasAdminRead;
                            }

                            if (_condition == PermissionActionCondition.And)
                            {
                                result &= result1;
                            }
                            else
                            {
                                result |= result1;
                            }
                        }

                        return result;
                    }
                case PermissionDomain.Device:
                    {
                        var action = _actions[0];
                        var result = false;
                        if (action == PermissionAction.Write)
                        {
                            result = permission.HasDeviceEdit;
                        }
                        if (action == PermissionAction.Read)
                        {
                            result = permission.HasDeviceRead;
                        }

                        for (int i = 1; i < _actions.Length; i++)
                        {
                            action = _actions[i];
                            var result1 = false;
                            if (action == PermissionAction.Write)
                            {
                                result1 = permission.HasDeviceEdit;
                            }
                            if (action == PermissionAction.Read)
                            {
                                result1 = permission.HasDeviceRead;
                            }

                            if (_condition == PermissionActionCondition.And)
                            {
                                result &= result1;
                            }
                            else
                            {
                                result |= result1;
                            }
                        }

                        return result;
                    }
                case PermissionDomain.Key:
                    {
                        var action = _actions[0];
                        var result = false;
                        if (action == PermissionAction.Write)
                        {
                            result = permission.HasKeyholderEdit;
                        }
                        if (action == PermissionAction.Read)
                        {
                            result = permission.HasKeyholderRead;
                        }

                        for (int i = 1; i < _actions.Length; i++)
                        {
                            action = _actions[i];
                            var result1 = false;
                            if (action == PermissionAction.Write)
                            {
                                result1 = permission.HasKeyholderEdit;
                            }
                            if (action == PermissionAction.Read)
                            {
                                result1 = permission.HasKeyholderRead;
                            }

                            if (_condition == PermissionActionCondition.And)
                            {
                                result &= result1;
                            }
                            else
                            {
                                result |= result1;
                            }
                        }

                        return result;
                    }
                case PermissionDomain.Space:
                    {
                        var action = _actions[0];
                        var result = false;
                        if (action == PermissionAction.Write)
                        {
                            result = permission.HasSpaceEdit;
                        }
                        if (action == PermissionAction.Read)
                        {
                            result = permission.HasSpaceRead;
                        }

                        for (int i = 1; i < _actions.Length; i++)
                        {
                            action = _actions[i];
                            var result1 = false;
                            if (action == PermissionAction.Write)
                            {
                                result1 = permission.HasSpaceEdit;
                            }
                            if (action == PermissionAction.Read)
                            {
                                result1 = permission.HasSpaceRead;
                            }

                            if (_condition == PermissionActionCondition.And)
                            {
                                result &= result1;
                            }
                            else
                            {
                                result |= result1;
                            }
                        }

                        return result;
                    }
                case PermissionDomain.Config:
                    {
                        var action = _actions[0];
                        var result = false;
                        if (action == PermissionAction.Write)
                        {
                            result = permission.HasConfigEdit;
                        }
                        if (action == PermissionAction.Read)
                        {
                            result = permission.HasConfigRead;
                        }

                        for (int i = 1; i < _actions.Length; i++)
                        {
                            action = _actions[i];
                            var result1 = false;
                            if (action == PermissionAction.Write)
                            {
                                result1 = permission.HasConfigEdit;
                            }
                            if (action == PermissionAction.Read)
                            {
                                result1 = permission.HasConfigRead;
                            }

                            if (_condition == PermissionActionCondition.And)
                            {
                                result &= result1;
                            }
                            else
                            {
                                result |= result1;
                            }
                        }

                        return result;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
    }
}
