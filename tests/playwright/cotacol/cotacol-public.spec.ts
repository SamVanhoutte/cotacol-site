import { test, expect, type Page } from '@playwright/test';

test('Verify page title', async ({ page }, testInfo) => {
  await page.goto('/stats');

  // Expect a title "to contain" a substring.
  await expect(page).toHaveTitle("Cotacol.cc");
  const screenshot = await page.screenshot({ quality: 50, type: 'png' })
  await testInfo.attach('screenshot', {
    body: screenshot,
    contentType: 'image/png',
  })
  //await page.screenshot({ path: 'screens/stats.png' });
});

// test('Col stats should return all cols', async ({ page }) => {
//   await page.goto('/colstats');

//   // Count the number of elements with an id starting with 'cotacol'
//   await page.waitForSelector('[id="cotacolitem"]', { timeout: 10000 });
//   const cotacolElements = await page.locator('[id="cotacolitem"]').count();
//   expect(cotacolElements).toBe(1000); // Verify the count is 1000
//   await page.screenshot({ path: 'screens/colstats.png' });
// });

// test('Leaderboard exists', async ({ page }) => {
//   await page.goto('/stats');

//   // Count the number of elements with an id starting with 'cotacol'
//   await page.waitForSelector('[id="cotacoluser"]', { timeout: 10000 });
//   const cotacolElements = await page.locator('[id="cotacoluser"]').count();
//   expect(cotacolElements).toBeGreaterThan(50); // Verify the count is 1000
//   await page.screenshot({ path: 'stats.png' });
// });

// test('Climbs list should work', async ({ page }) => {
//   await page.goto('/climbs');

//   // Count the number of elements with an id starting with 'cotacol'
//   const cotacolElements = await page.locator('[id^="cotacol"]').count();
//   expect(cotacolElements).toBe(1000); // Verify the count is 1000

//   // Generate a random number between 1 and 1000
//   const randomNumber = Math.floor(Math.random() * 1000) + 1;
//   await page.screenshot({ path: 'screens/climbs.png' });

//   // Click the element with id 'cotacol-{randomnumber}'
//   await page.locator(`#cotacol-${randomNumber}`).click();
// });

// test('Col list to random detail', async ({ page }) => {
//   const cotacolId = Math.floor(Math.random() * 1000) + 1;
//   await page.goto(`/cotacol/${cotacolId}`);
//   const elevationElement = page.locator('#cotacolelevation');
//   await expect(page.locator('#cotacolelevation')).toBeVisible(); // Assert that the element is visible
//   await expect(page.locator('#cotacolgrade')).toBeVisible(); // Assert that the element is visible
//   await expect(page.locator('#cotacoldistance')).toBeVisible(); // Assert that the element is visible
//   await expect(page.locator('#cotacolsurface')).toBeVisible(); // Assert that the element is visible
//   await expect(page.locator('#cotacoldone')).toHaveCount(0); // Assert that the element is visible
//   await expect(page.locator('#map1')).toBeVisible(); // Assert that the element is visible
//   await page.screenshot({ path: 'screens/climbdetail.png' });

// });