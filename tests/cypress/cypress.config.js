module.exports = {
  e2e: {
    experimentalModifyObstructiveThirdPartyCode: true,
    experimentalStudio: true,
    baseUrl: "https://localhost:7259/",
    setupNodeEvents(on, config) {
      // implement node event listeners here
    },
  },
  env: {
    gmail_user: "cotacol.hunter@gmail.com",
    gmail_password: ""
  },
  projectId: "tuaopt",
};
