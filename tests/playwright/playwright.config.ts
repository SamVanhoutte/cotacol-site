import { PlaywrightTestConfig, defineConfig, devices } from "@playwright/test";
/**
 * Read environment variables from file.
 * https://github.com/motdotla/dotenv
 */
// import dotenv from 'dotenv';
// import path from 'path';
// dotenv.config({ path: path.resolve(__dirname, '.env') });

/**
 * See https://playwright.dev/docs/test-configuration.
 */
const config: PlaywrightTestConfig<{}, {}> = {
  testDir: './cotacol',
  /* Run tests in files in parallel */
  fullyParallel: true,
  /* Fail the build on CI if you accidentally left test.only in the source code. */
  forbidOnly: !!process.env.CI,
  /* Retry on CI only */
  retries: process.env.CI ? 2 : 0,
  /* Opt out of parallel tests on CI. */
  workers: process.env.CI ? 1 : undefined,
  snapshotDir: './snapshots',
  /* Reporter to use. See https://playwright.dev/docs/test-reporters */
  reporter: 'html',
  /* Shared settings for all the projects below. See https://playwright.dev/docs/api/class-testoptions. */
  use: {
    /* Base URL to use in actions like `await page.goto('/')`. */
    baseURL: 'https://www.cotacol.cc/',

    /* Collect trace when retrying the failed test. See https://playwright.dev/docs/trace-viewer */
    trace: 'on-first-retry',
  },

  /* Configure projects for major browsers */
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },

    {
      name: 'firefox',
      use: { ...devices['Desktop Firefox'] },
    },

    {
      name: 'webkit',
      use: { ...devices['Desktop Safari'] },
    },

    /* Test against mobile viewports. */
    // {
    //   name: 'Mobile Chrome',
    //   use: { ...devices['Pixel 5'] },
    // },
    // {
    //   name: 'Mobile Safari',
    //   use: { ...devices['iPhone 12'] },
    // },

    /* Test against branded browsers. */
    // {
    //   name: 'Microsoft Edge',
    //   use: { ...devices['Desktop Edge'], channel: 'msedge' },
    // },
    // {
    //   name: 'Google Chrome',
    //   use: { ...devices['Desktop Chrome'], channel: 'chrome' },
    // },
  ],

  /* Run your local dev server before starting the tests */
  // webServer: {
  //   command: 'npm run start',
  //   url: 'http://127.0.0.1:3000',
  //   reuseExistingServer: !process.env.CI,
  // },
};

if (config.reporter && config.reporter instanceof Array) {
  let runUrl = "";
  if (process.env.GITHUB_RUN_ID) {
    const runId = process.env.GITHUB_RUN_ID;
    const repo = process.env.GITHUB_REPOSITORY;
    runUrl = `${process.env.GITHUB_SERVER_URL}/${repo}/actions/runs/${runId}`;
  }

  if (process.env.NODE_ENV !== "development") {
    config.reporter.push(["html"]);
    config.reporter.push([
      "@estruyf/github-actions-reporter",
      {
        showError: true,
        azureStorageSAS: process.env.AZURE_STORAGE_SAS,
        azureStorageUrl: process.env.AZURE_STORAGE_URL,
      },
    ]);
    // config.reporter.push([
    //   "playwright-msteams-reporter",
    //   <MsTeamsReporterOptions>{
    //     webhookUrl: process.env.M365_WEBHOOK_URL,
    //     webhookType: "powerautomate",
    //     linkToResultsUrl: runUrl,
    //     mentionOnFailure: process.env.M365_USERNAME,
    //   },
    // ]);
  } else {
    config.reporter.push(["html"]);
  }
}

/**
 * See https://playwright.dev/docs/test-configuration.
 */
export default defineConfig(config);
