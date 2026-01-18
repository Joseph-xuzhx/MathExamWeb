window.appLocalStorage = {
  get: function (key) {
    try { return window.localStorage.getItem(key); } catch { return null; }
  },
  set: function (key, value) {
    try { window.localStorage.setItem(key, value); return true; } catch { return false; }
  },
  remove: function (key) {
    try { window.localStorage.removeItem(key); return true; } catch { return false; }
  }
};
