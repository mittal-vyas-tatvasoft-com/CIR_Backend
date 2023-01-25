using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Common.Helper
{
    public class SystemMessages
    {
        #region Common
        public static string msgInvalidId = "Invalid Input Id.";
        public static string msgBadRequest = "Bad Request, issue in client request";
        public static string msgNotFound = "Requested {0} were not found.";
        public static string msgDataExists = "{0} Already Exists.";
        public static string msgEnterValidData = "Please enter valid Data.";
        public static string msgDataSavedSuccessfully = "{0} Saved Successfully.";
        public static string msgDataDisabledSuccessfully = "{0} Disabled Successfully.";
        public static string msgDataDeletedSuccessfully = "{0} Deleted Successfully.";
        public static string msgUpdatingDataError = "An error occurred while updating {0}.";
        public static string msgDataUpdatedSuccessfully = "{0} Updated Successfully.";
        public static string msgIdNotFound = "{0} with given id not found";
        public static string msgAddingDataError = "An error occurred while adding new {0}.";
        #endregion

        #region GlobalConfigurationHolidays
        public static string msgSelectXlsxOrCSVFile = "Select only .xlsx or .csv file.";
        #endregion

        #region Users
        public static string msgUnableToFindAppropriateUsers = "Database was unable to find appropriate users.";
        #endregion

        #region Role
        public static string msgCannotRemoveRecord = "You can not remove this record because it is currently in use.";
        #endregion

        #region login
        public static string msgAccountIsLocked = "Your Account is locked. To Unlock this account contact to Aramex Support team.";
        public static string msgInvalidUserNameOrPassword = "Invalid username or password.";
        public static string msgTokenNotGenerated = "Token not generated.";
        public static string msgEnterValidUserNameAndEmail = "Please enter valid username and email.";
        public static string msgPasswordChangedSuccessfully = "Password Changed Successfully.";
        public static string msgIncorrectOldPassword = "Old Password Is InCorrect.";
        public static string msgInvalidEmailAddress = "Invalid Email Address.";
        public static string msgSendNewPasswordOnMail = "Successfully send new password on your mail,please check once!";
        #endregion
    }
}
