---
layout: default
navigation: home
---
Ragu is the [OpenSimulator] space manager for a [Basil] viewer.

<ul class="post-list">
  {% for post in site.posts %}
    {% if post.status == "publish" %}
      {% include one-post.html thepost=post thecontent=post.content ownpage="no" %}
    {% endif %}
  {% endfor %}

[OpenSimulator]: http://opensimulator.org/
[Basil]:  http://misterblue.github.io/basil/
[Pesto]:  http://misterblue.github.io/pesto/
[Loc-Loc]: http://misterblue.github.io/loc-loc/
[Ragu]: http://misterblue.github.io/ragu/
<!-- vim: ts=2 sw=2 ai et nospell
-->
