window.blazorWpBooking = {
  readCookie: function (name) {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? decodeURIComponent(match[2]) : null;
  },
  deleteCookie: function (name) {
    document.cookie = name + '=; Max-Age=0; path=/';
  }
};