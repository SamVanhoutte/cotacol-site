import { test as setup, expect } from '@playwright/test';
import path from 'path';

const authFile = path.join(__dirname, '../.auth/user.json');

// setup('authenticateStrava', async ({ page }) => {
//   // Perform authentication steps. Replace these actions with your own.
//   await page.goto('https://www.strava.com/login');
//   await page.waitForTimeout(20000);
//   await page.getByLabel('Email').last().fill('sam.vanhoutte@marchitec.be');
//   await page.waitForTimeout(300);
//   await page.getByLabel('Password').last().fill('sam@cotacol.cc');
//   await page.waitForTimeout(2000);
//   await page.getByRole('button', { name: 'Log in' }).click();
//   // Wait until the page receives the cookies.
//   // Sometimes login flow sets cookies in the process of several redirects.
//   // Wait for the final URL to ensure that the cookies are actually set.
//   await page.waitForTimeout(3000);
//   await page.waitForURL('https://www.strava.com/');
//   // Alternatively, you can wait until the page reaches a state where all cookies are set.
//   // await expect(page.getByRole('button', { name: 'View profile and more' })).toBeVisible();

//   // End of authentication steps.
//   await page.context().storageState({ path: authFile });
// });

setup('authenticateGoogle', async ({  }) => {
  const ua = require('user-agents');
  const { chromium } = require('playwright');
  const browser = await chromium.launch({ 
    headless: false,
    args: [
      '--disable-blink-features=AutomationControlled',
      '--no-sandbox',         // May help in some environments
      '--disable-web-security', // Not recommended for production use
      '--disable-infobars',    // Prevent infobars
      '--disable-extensions',   // Disable extensions
      '--start-maximized',      // Start maximized
      '--window-size=1280,720'  // Set a specific window size
    ]
});
  const userAgent = new ua({
    platform: 'MacIntel', // 'Win32', 'Linux ...'
    deviceCategory: 'desktop', // 'mobile', 'tablet'
  });
  
  const context = await browser.newContext({
     userAgent: userAgent.toString(),
     viewport: { width: 1280, height: 720 },
     deviceScaleFactor: 1,
  });
  
  const page = await context.newPage();

  // First authenticate against Google
  await page.goto('https://accounts.google.com/');
  await page.getByLabel('Email or phone').last().fill('cotacol.hunter@gmail.com');
  await page.waitForTimeout(300);
  await page.getByRole('button', { name: 'Next' }).click();
  await page.waitForURL('https://accounts.google.com/v3/signin/challenge/**');
  await page.getByLabel('Enter your password').last().fill('');
  await page.waitForTimeout(300);
  await page.getByRole('button', { name: 'Next' }).click();
  await page.waitForTimeout(1000);

  // Now authenticate against strava
  await page.goto('https://www.strava.com/login');
  await page.waitForURL('https://accounts.google.com/v3/signin/challenge/**');
  await page.getByRole('button', { name: 'Sign In With Google' }).click();
  await page.waitForURL('https://www.strava.com/**');

  // End of authentication steps.
  await page.context().storageState({ path: authFile });
});