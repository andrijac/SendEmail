SendEmail
=========

Send email from command line and keep credentials in encrypted file.

You have to write credentials before sending email.
To write credentials, see example below "Write credentials".

**Usage:**

Send email:<br />
`SendEmail.exe "security key" "to address" "email subject" "email body"`

Write credentials:<br />
`SendEmail.exe -w "from adress" "from password" "security key"`

**Macros**

- `{current_user}` - Current user
- `{now}` - Current time in format "yyyy-MM-dd HH:mm"

**Examples:**

Send email: <br />
`SendEmail.exe securitykey to@domain.com "subject" "body"`

Write credentials: <br />
`SendEmail.exe -w from@domain.com fromPassword securitykey`


**Notes:**

- Security key has to be 8 characters (for current implementation).
- Credentials are written in encrypted file `sendemail.creds`.