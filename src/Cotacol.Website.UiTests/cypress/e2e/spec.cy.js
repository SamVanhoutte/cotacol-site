describe('Cotacol information', () => {
  it('Col list to detail', function() {
    const cotacolId = '1' + Math.random().toString().substr(2, 2)
    cy.visit('/climbs');
    cy.get(`#cotacol-${cotacolId}`).click();

    cy.get('#cotacolelevation').should('exist', 'cotacol elevation is required');
    cy.get('#cotacolgrade').should('exist');
    cy.get('#cotacoldistance').should('exist');
    cy.get('#cotacolsurface').should('exist');
    cy.get('#cotacoldone').should('not.exist');
    cy.get('#map1').should('exist');
  });
})