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
  //globalSetup: ".lib/global-setup.ts",
  testDir: './cotacol',
  //timeout: 5000,
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
  reporter: [
    ['html'],
    ['playwright-ctrf-json-reporter', {
      outputFile: 'ctrf-output.json', // Optional: Output file name. Defaults to 'ctrf-report.json'.
      outputDir: 'tests/playwright/ctrf',
      screenshot: true,
      annotations: true
    }]
  ],
  
  /* Shared settings for all the projects below. See https://playwright.dev/docs/api/class-testoptions. */
  use: {
    /* Base URL to use in actions like `await page.goto('/')`. */
    baseURL: 'https://localhost:7259/', //'https://www.cotacol.cc/',

    /* Collect trace when retrying the failed test. See https://playwright.dev/docs/trace-viewer */
    trace: 'on-first-retry',
    actionTimeout: 15000, // Timeout for individual actions (e.g., click, fill)
    navigationTimeout: 20000, // Timeout for navigation actions (e.g., goto, waitForURL)
  },

  /* Configure projects for major browsers */
  projects: [
    // { name: 'setup', testMatch: 'auth.setup.ts' },
    {
      name: 'chromium',
      // dependencies: ["setup"],
      use: { ...devices['Desktop Chrome'] ,
        // Use prepared auth state.
          storageState: 'tests/playwright/.auth/user.json'
        },
    },

    // {
    //   name: 'firefox',
    //   dependencies: ["setup"],
    //   use: { ...devices['Desktop Firefox'] ,
    //     // Use prepared auth state.
    //    storageState: 'tests/playwright/.auth/user.json'
    // },

    // {
    //   name: 'webkit',
    //   dependencies: ["setup"],
    //   use: { ...devices['Desktop Safari'] ,
    //     // Use prepared auth state.
    //     storageState: 'tests/playwright/.auth/user.json'
    // },
  ],
};

export default defineConfig(config);
