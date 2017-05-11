using BeautySNS.Domain.Code.Interfaces;
using BeautySNS.Domain.DAO.Interfaces;
using BeautySNS.Domain.Model;
using BeautySNS.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySNS.Domain.DAO
{
    public class ProfileDAO : IProfileDAO
    {
        private readonly BSNSContext _db; //creates an instance of the database
        private IUserSession userSession;
        private IAlertService alertService;

        public ProfileDAO(BSNSContext db, IUserSession userSession, IAlertService alertService)
        {
            _db = db;
            this.userSession = userSession;
            this.alertService = alertService;
        }

        //returns a profile based on its accountID
        public Profile fetchByAccountID(int accountID)
        {
            return _db.Profiles.Where(p => p.accountID == accountID).FirstOrDefault();
        }

        //returns a profile based on its JobID
        public Profile fetchByJob(int jobID)
        {
            return _db.Profiles.Where(p => p.jobID == jobID).FirstOrDefault();
        }

        public void CreateProfile(Profile profile)
        {
            profile.createDate = DateTime.Now;
            _db.Profiles.Add(profile);
            alertService.AddProfileCreatedAlert();
            _db.SaveChanges();

        }

        public void UpdateProfile(Profile profile)
        {
            profile.lastUpdateDate = DateTime.Now;

            if (profile.accountID > 0)//if the profile exists, edit the profile
            {
                Profile originalProfile = _db.Profiles.Find(profile.accountID);
                originalProfile.accountID = originalProfile.accountID;
                originalProfile.profileID = profile.profileID;
                originalProfile.jobID = profile.jobID;
                originalProfile.avatar = profile.avatar;
                originalProfile.avatarMIMEType = profile.avatarMIMEType;
                originalProfile.aboutMe = profile.aboutMe;
                originalProfile.education = profile.education;
                originalProfile.experience = profile.experience;
                originalProfile.location = profile.location;
                originalProfile.website = profile.website;
                originalProfile.Account.firstName = profile.Account.firstName;
                originalProfile.Account.lastName = profile.Account.lastName;
                originalProfile.Account.birthDate = profile.Account.birthDate;
                originalProfile.Account.dateUpdated = profile.lastUpdateDate;
                alertService.AddProfileModifiedAlert();
                _db.SaveChanges();
             
            }

        }

       
    }
}
