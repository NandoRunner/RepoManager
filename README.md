# RepoManager

Windows forms application that manages your local git repositories. 

Below is the list of funcionalities:
- List all
- List repos with pending changes
- Send e-mail with selcted list of repos throught Gmail
- Ignore a repo from future listing
- Restore repos from GitHub, Bitbucket and GitLab


Requirements:
1) Git for Windows
https://gitforwindows.org/

2) GitHub personal access token (PAT)
https://github.com/settings/tokens

Run command:
git config --global credential.helper manager

The first time you are pushing to a repo, a popup will ask for your credentials: username and your PAT.


Email:
For sending an email with you list of repos, you need to use your gmail credentials and activate "less secure apps":
https://myaccount.google.com/u/2/lesssecureapps

Sugestion: Do not use your main google account, use a secondary one that there is no problem on activating "less secure apps".

Remenber that Google automatically deactivates "less secure apps" if it is not being used.



