// import { test, expect, type Page, TestInfo } from '@playwright/test';
// import sharp from 'sharp';

// test.describe('Authenticated tests', () => {
//   test.use({ storageState: "tests/playwright/.auth/user.json" });


//   // Setup for every test
//   test.beforeEach(async ({ page }) => {
//     await page.goto(process.env.BaseUrl || "https://localhost:7259/", {
//       waitUntil: "domcontentloaded",
//     });
//   });

//   test('Achievements test', async ({ page }, testInfo) => {
//     await page.goto('/achievements');
//     await page.getByRole('link', { name: 'My Achievements' }).first().click();
//     const yearPic = page.locator('[id^="cotacolwrapped"]');
//     await yearPic.screenshot({ path: 'screens/yearpic.png' });
//     await imageShouldContainColor('screens/yearpic.png', { r: 4, g: 20, b: 60 }, 5);
//     await screenshotPage(page, testInfo, 'achievements');
//   });


//   async function screenshotPage(page: Page, testInfo: TestInfo, name: string) {
//     await page.screenshot({ path: `tests/playwright/screens/${name}.png` });
//     const screenshot = await page.screenshot({ type: 'png' });
//     await testInfo.attach(name, {
//       body: screenshot,
//       contentType: 'image/png',
//     });
//   }

//   async function imageShouldContainColor(imageFile: string, targetColor: { r: number, g: number, b: number }, difference = 5) {
//     // Load the screenshot using sharp
//     const image = sharp(imageFile);
//     const { data, info } = await image.raw().toBuffer({ resolveWithObject: true });

//     // Check if the color is present in the screenshot
//     let colorFound = false;
//     for (let i = 0; i < data.length; i += 3) {
//       const r = data[i];
//       const g = data[i + 1];
//       const b = data[i + 2];
//       if (
//         Math.abs(r - targetColor.r) <= difference &&
//         Math.abs(g - targetColor.g) <= difference &&
//         Math.abs(b - targetColor.b) <= difference
//       ) {
//         colorFound = true;
//         break;
//       }
//     }

//     // Validate that the color is found
//     expect(colorFound).toBe(true);
//   }
// });