window.blazorWpBooking = {
  readCookie: function (name) {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? decodeURIComponent(match[2]) : null;
  },
  deleteCookie: function (name) {
    document.cookie = name + '=; Max-Age=0; path=/';
  },
  setCultureCookieAndReload: function (cookieName, cookieValue) {
    // Set the culture cookie with a 1-year expiration
    const expirationDate = new Date();
    expirationDate.setFullYear(expirationDate.getFullYear() + 1);
    document.cookie = cookieName + '=' + cookieValue + '; expires=' + expirationDate.toUTCString() + '; path=/';
    
    // Reload the page to apply the new culture
    window.location.reload();
  }
};