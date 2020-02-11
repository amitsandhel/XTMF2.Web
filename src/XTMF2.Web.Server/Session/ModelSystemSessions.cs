//     Copyright 2017-2020 University of Toronto
// 
//     This file is part of XTMF2.
// 
//     XTMF2 is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     XTMF2 is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with XTMF2.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using XTMF2.Editing;

namespace XTMF2.Web.Server.Session
{
    /// <summary>
    ///     Holds the session state for an active user. Maintains references to projects and other
    ///     related items.
    /// </summary>
    public class ModelSystemSessions
    {
        /// <summary>
        ///     User dictionary of active model system sessions.
        /// </summary>
        /// <returns></returns>
        public Dictionary<User, Dictionary<Project, List<ModelSystemSession>>> Sessions { get; } =
            new Dictionary<User, Dictionary<Project, List<ModelSystemSession>>>();

        /// <summary>
        ///     Clears all model system sessions for the associated user
        /// </summary>
        /// <param name="user"></param>
        public void ClearSessionsForUser(User user)
        {
            if (Sessions.ContainsKey(user))
            {
                //dispose each session
                foreach (var project in Sessions[user].Values)
                {
                    foreach (var modelSystem in project)
                    {
                        modelSystem.Dispose();
                    }
                }
                Sessions[user].Clear();
            }
        }

        /// <summary>
        ///     Adds / tracks a session for the associated user.
        /// </summary>
        /// /// <param name="user"></param>
        /// <param name="session"></param>
        public void TrackSessionForUser(User user, Project project, ModelSystemSession session)
        {
            if (!Sessions.ContainsKey(user))
            {
                Sessions[user] = new Dictionary<Project, List<ModelSystemSession>>();
            }
            if (!Sessions[user].ContainsKey(project))
            {
                Sessions[user][project] = new List<ModelSystemSession>();
            }
            Sessions[user][project].Add(session);
        }

        /// <summary>
        /// Gets a model system session for the associate model system and project
        /// </summary>
        /// <param name="user"></param>
        /// <param name="project"></param>
        /// <param name="modelSystemName"></param>
        /// <returns></returns>
        public ModelSystemSession GetModelSystemSession(User user, Project project, ModelSystemHeader modelSystemHeader)
        {
            if (!Sessions.ContainsKey(user) && !Sessions[user].ContainsKey(project))
            {
                return null;
            }
            return Sessions[user][project].Find(ms =>
            {
                return ms.ModelSystemHeader == modelSystemHeader;
            });
        }
    }
}