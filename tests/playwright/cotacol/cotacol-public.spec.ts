import { test, expect, type Page, TestInfo } from '@playwright/test';

// Setup for every test
test.beforeAll(async ({ browser }) => {
  let page: Page;
  // Set up new page , take the BaseUrl parameter from input
  page = await browser.newPage();
  await page.goto(process.env.BaseUrl || "https://localhost:7259/", {
    waitUntil: "domcontentloaded",
  });

});

test('Verify page title', async ({ page }, testInfo) => {
  await page.goto('/');

  // Expect a title "to contain" a substring.
  await expect(page).toHaveTitle("Cotacol.cc");
  await screenshotPage(page, testInfo, 'home');
});

test('Col stats should return all cols', async ({ page }, testInfo) => {
  await page.goto('/colstats');

  // Count the number of elements with an id starting with 'cotacol'
  await page.waitForSelector('[id="cotacolitem"]', { timeout: 10000 });
  const cotacolElements = await page.locator('[id="cotacolitem"]').count();
  expect(cotacolElements).toBe(1000); // Verify the count is 1000
  await screenshotPage(page, testInfo, 'colstats');
});

// test('Leaderboard exists', async ({ page }, testInfo) => {
//   await page.goto('/stats');

//   // Count the number of elements with an id starting with 'cotacol'
//   await page.waitForSelector('[id="cotacoluser"]', { timeout: 10000 });
//   const cotacolElements = await page.locator('[id="cotacoluser"]').count();
//   expect(cotacolElements).toBeGreaterThan(50); // Verify the count is 1000
//   await screenshotPage(page, testInfo, 'stats');
// });

test('Climbs list should work', async ({ page }, testInfo) => {
  await page.goto('/climbs');

  // Count the number of elements with an id starting with 'cotacol'
  const cotacolElements = await page.locator('[id^="cotacol"]').count();
  expect(cotacolElements).toBe(1000); // Verify the count is 1000

  // Generate a random number between 1 and 1000
  const randomNumber = Math.floor(Math.random() * 1000) + 1;
  await screenshotPage(page, testInfo, 'climbs');

  // Click the element with id 'cotacol-{randomnumber}'
  await page.locator(`#cotacol-${randomNumber}`).click();
});

test('Col list to random detail', async ({ page }, testInfo) => {
  const cotacolId = Math.floor(Math.random() * 1000) + 1;
  await page.goto(`/cotacol/${cotacolId}`);
  const elevationElement = page.locator('#cotacolelevation');
  await expect(page.locator('#cotacolelevation')).toBeVisible(); // Assert that the element is visible
  await expect(page.locator('#cotacolgrade')).toBeVisible(); // Assert that the element is visible
  await expect(page.locator('#cotacoldistance')).toBeVisible(); // Assert that the element is visible
  await expect(page.locator('#cotacolsurface')).toBeVisible(); // Assert that the element is visible
  await expect(page.locator('#cotacoldone')).toHaveCount(0); // Assert that the element is visible
  await expect(page.locator('#map1')).toBeVisible(); // Assert that the element is visible
  await screenshotPage(page, testInfo, 'climbdetail');

});


async function screenshotPage(page: Page, testInfo: TestInfo, name: string) {
  await page.screenshot({ path: `screens/${name}.png` });
  const screenshot = await page.screenshot({ type: 'png' });
  await testInfo.attach(name, {
    body: screenshot,
    contentType: 'image/png',
  });
}