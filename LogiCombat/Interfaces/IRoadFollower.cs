public interface IRoadFollower
{
    RoadNode FindStartNode();
    RoadNode FindNextNode(RoadNode currentNode);
}